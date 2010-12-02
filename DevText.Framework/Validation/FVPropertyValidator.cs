using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using FluentValidation;
using FluentValidation.Validators;

namespace DevText.Framework.Validation
{
	public class FVPropertyValidator:ModelValidator
	{
		protected readonly IPropertyValidator validator;

		protected bool ShouldValidate { get; set; }

		public FVPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext,IPropertyValidator validator)
			:base(metadata,controllerContext)
		{
			this.validator = validator;
		}

		public override IEnumerable<ModelValidationResult> Validate(object container)
		{
			if (ShouldValidate)
			{
				var context = new PropertyValidatorContext(Metadata.PropertyName, container, Metadata.Model, Metadata.PropertyName);
				var result = validator.Validate(context);
				foreach (var failure in result)
				{
					yield return new ModelValidationResult { Message = failure.ErrorMessage };
				}
			}
		}

		public static ModelValidator Create(ModelMetadata meta, ControllerContext context, IPropertyValidator validator)
		{
			return new FVPropertyValidator(meta, context, validator);
		}

		protected bool TypeAllowsNullValue(Type type)
		{
			return (!type.IsValueType || Nullable.GetUnderlyingType(type) != null);
		}
	}

	internal class RequiredFVPropertyValidator:FVPropertyValidator
	{
		public RequiredFVPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, IPropertyValidator validator)
			: base(metadata, controllerContext, validator)
		{
			bool isNonNullableValueType = !TypeAllowsNullValue(metadata.ModelType);
			bool nullWasSpecified = metadata.Model == null;

			ShouldValidate = isNonNullableValueType && nullWasSpecified;
		}

		public new static ModelValidator Create(ModelMetadata meta, ControllerContext context,IPropertyValidator validator)
		{
			return new RequiredFVPropertyValidator(meta, context, validator);
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			return new[] { new ModelClientValidationRequiredRule(validator.ErrorMessageSource.BuildErrorMessage()) };
		}

		public override bool IsRequired
		{
			get
			{
				return true;
			}
		}
	}

	internal class StringLengthFVPropertyValidator:FVPropertyValidator
	{
		private ILengthValidator LengthValidator
		{
			get { return (ILengthValidator)validator; }
		}

		public StringLengthFVPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, IPropertyValidator validator)
			: base(metadata, controllerContext, validator)
		{
			ShouldValidate = false;
		}
		public new static ModelValidator Create(ModelMetadata meta, ControllerContext context,IPropertyValidator validator)
		{
			return new StringLengthFVPropertyValidator(meta, context, validator);
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			return new[] {new ModelClientValidationStringLengthRule(LengthValidator.ErrorMessageSource.BuildErrorMessage(),LengthValidator.Min,LengthValidator.Max)};
		}
	}

	internal class RegularExpressionFVPropertyValidator:FVPropertyValidator
	{
		IRegularExpressionValidator RegexValidator
		{
			get { return (IRegularExpressionValidator)validator; }
		}

		public RegularExpressionFVPropertyValidator(ModelMetadata metadata,ControllerContext controllerContext,IPropertyValidator validator)
			:base (metadata,controllerContext,validator)
		{
			ShouldValidate = false;
		}

		public new static ModelValidator Create(ModelMetadata meta, ControllerContext context,IPropertyValidator validator)
		{
			return new RegularExpressionFVPropertyValidator(meta, context, validator);
		}

		public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
		{
			return new[] { new ModelClientValidationRegexRule(RegexValidator.ErrorMessageSource.BuildErrorMessage(),RegexValidator.Expression)};
		}
	}
	// will extend Fluentvalidation to enable this function :)
	//internal class RangeFVPropertyValidator:FVPropertyValidator
	//{
	//    public RangeFVPropertyValidator(ModelMetadata metadata, ControllerContext controllerContext, IPropertyValidator validator)
	//        :base(metadata,controllerContext,validator)
	//    {
	//        ShouldValidate = false;
	//    }

	//    RegularExpressionValidator

	//    public new static ModelValidator Create(ModelMetadata meta,ControllerContext context,IPropertyValidator validator)
	//    {
	//        return new RangeFVPropertyValidator(meta, context, validator);
	//    }

	//    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
	//    {
	//        return new[] {new ModelClientValidationRangeRule(validator.ErrorMessageSource.BuildErrorMessage(),minValue ,maxValue)}
	//    }
	//}
}
