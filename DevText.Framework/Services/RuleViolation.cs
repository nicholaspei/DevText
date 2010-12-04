using System.Diagnostics;

namespace DevText.Framework.Services
{
    public class RuleViolation
    {

        public RuleViolation(string parameterName,string errorMessage)
        {
            ParameterName = parameterName;
            ErrorMessage = errorMessage;
        }

        public string ParameterName
        {
            get;
            private set;
        }

        public string ErrorMessage
        {
            get;
            private set;
        }
    }
}
