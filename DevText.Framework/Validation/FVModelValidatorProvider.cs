using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Validators;

namespace DevText.Framework.Validation
{
    public delegate ModelValidator FVModelValidationFactory(ModelMetadata metadata,ControllerContext context,IPropertyValidator validator);

    /// <summary>
    /// Fluentvalidation 
    /// </summary>
    public class FVModelValidatorProvider:ModelValidatorProvider
    {
        readonly IValidatorFactory validatorFactory;
        public bool AddImplicitRequiredValidator { get; set; }

        private Dictionary<Type, FVModelValidationFactory> validatorFactories = new Dictionary<Type, FVModelValidationFactory>()
        {
            {typeof(INotNullValidator),RequiredFVPropertyValidator.Create},
            {typeof(INotEmptyValidator),RequiredFVPropertyValidator.Create},
            {typeof(IRegularExpressionValidator),RegularExpressionFVPropertyValidator.Create},
            {typeof(ILengthValidator),StringLengthFVPropertyValidator.Create}
        };

        public FVModelValidatorProvider(IValidatorFactory validatorFactory)
        {
            AddImplicitRequiredValidator = true;
            this.validatorFactory = validatorFactory;
        }
        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (!IsValidatiingProperty(metadata))
            {
                return GetValidatorsForProperty(metadata, context, validatorFactory.GetValidator(metadata.ContainerType));
            }
            return GetValidatorsForModel(metadata, context, validatorFactory.GetValidator(metadata.ModelType));
        }

        IEnumerable<ModelValidator> GetValidatorsForProperty(ModelMetadata metadata, ControllerContext context, IValidator validator)
        {
            var modelValidators = new List<ModelValidator>();

            if (validator != null)
            {
                var descriptor = validator.CreateDescriptor();

                var validators = descriptor.GetValidatorsForMember(metadata.PropertyName)
                    .Where(x => x.SupportsStandaloneValidation);

                foreach (var propertyValidator in validators)
                {
                    modelValidators.Add(GetModelValidator(metadata, context, propertyValidator));
                }
            }

            if (metadata.IsRequired && AddImplicitRequiredValidator)
            {
                bool hasRequiredValidators = modelValidators.Any(x => x.IsRequired);

                //Is the model is 'Required' then we assume it must have a NotNullValidator. 
                //This is consistent with the behaviour of the DataAnnotationsModelValidatorProvider
                //which silently adds a RequiredAttribute

                if (!hasRequiredValidators)
                {
                    modelValidators.Add(CreateNotNullValidatorForProperty(metadata, context));
                }
            }

            return modelValidators;
        }

        private ModelValidator GetModelValidator(ModelMetadata meta, ControllerContext context, IPropertyValidator propertyValidator)
        {
            var type = propertyValidator.GetType();

            var factory = validatorFactories
                .Where(x => x.Key.IsAssignableFrom(type))
                .Select(x => x.Value)
                .FirstOrDefault() ??  RequiredFVPropertyValidator.Create;

            return factory(meta, context, propertyValidator);
        }

        ModelValidator CreateNotNullValidatorForProperty(ModelMetadata metadata,ControllerContext cc)
        {
            return RequiredFVPropertyValidator.Create(metadata, cc, new NotNullValidator());
        }

        IEnumerable<ModelValidator> GetValidatorsForModel(ModelMetadata metadata, ControllerContext context,IValidator validator)
        {
            if (validator != null)
            {
                yield return new FVModelValidator(metadata, context, validator);
            }
        }

        bool IsValidatiingProperty(ModelMetadata metadata)
        {
            return metadata.ContainerType != null && !string.IsNullOrEmpty(metadata.PropertyName);
        }
    }
}
