using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using FluentValidation;
using FluentValidation.Results;

namespace DevText.Framework.Validation
{
    public static class ValidationResultExtension
    {
        /// <summary>
        /// Stores the errors in a ValidationResult object to the specified modelstate dictionary.
        /// </summary>
        /// <param name="result">The validation result to store</param>
        /// <param name="modelState">The ModelStateDictionary to store the errors in.</param>
        /// <param name="prefix">An optional prefix, If committed,the property names will be the keys. if specified,the prefix will be concatennated to the propertiy name with a period. Eg "user.Name"</param>
        public static void AddToModelState(this ValidationResult result,ModelStateDictionary modelState,string prefix)
        {
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    string key = string.IsNullOrEmpty(prefix) ? error.PropertyName : prefix + "." + error.PropertyName;
                    modelState.AddModelError(key, error.ErrorMessage);
                    // To work around an issue with MVC: SetModelValue must be called if AddModelError is called.
                    modelState.SetModelValue(key, new ValueProviderResult(error.AttemptedValue ?? "", (error.AttemptedValue ?? "").ToString(), CultureInfo.CurrentCulture));
                }
            }
        }
    }
}
