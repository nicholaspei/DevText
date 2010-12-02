using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace DevText.Framework.Validation
{
    /// <summary>
    /// This is a Fluentvalidation class, find the source code here : http://fluentvalidation.codeplex.com
    /// </summary>
    internal class FVModelValidator :ModelValidator
    {
        readonly IValidator validator;

        public FVModelValidator(ModelMetadata metadata, ControllerContext controllerContext,IValidator validator)
            :base(metadata,controllerContext)
        {
            this.validator = validator;
        }
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            if (Metadata.Model != null)
            {
                var result = validator.Validate(Metadata.Model);

                if (!result.IsValid)
                {
                    return ConvertValidationResultToModelValidationResults(result);
                }
            }
            return Enumerable.Empty<ModelValidationResult>();
        }

        protected virtual IEnumerable<ModelValidationResult> ConvertValidationResultToModelValidationResults(ValidationResult result)
        {
            return result.Errors.Select(x => new ModelValidationResult
            {
                MemberName = x.PropertyName,
                Message = x.ErrorMessage
            });
        }
    }
}
