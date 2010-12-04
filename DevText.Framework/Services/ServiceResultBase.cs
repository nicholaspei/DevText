using System.Collections.Generic;
using System.Diagnostics;

namespace DevText.Framework.Services
{
    public abstract class ServiceResultBase
    {
        protected ServiceResultBase(IEnumerable<RuleViolation> ruleViolations)
        {
            RuleViolations = new List<RuleViolation>(ruleViolations);
        }
        
        public IList<RuleViolation> RuleViolations
        {
            get;
            private set;
        }
 
    }
}
