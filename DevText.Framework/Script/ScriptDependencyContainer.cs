using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Script
{
    public class ScriptDependencyContainer
    {
        public string ReleaseSuffix { get; set; }
        public string DebugSuffix { get; set; }

        private List<ScriptDependency> _knownDependencies = new List<ScriptDependency>();
        public List<ScriptDependency> Dependencies { get { return _knownDependencies; } }
    }
}
