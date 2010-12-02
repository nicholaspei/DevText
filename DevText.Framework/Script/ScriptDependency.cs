using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevText.Framework.Script
{
     public class ScriptDependency
    {
         public string ScriptName { get; set; }
         public string ScriptPath { get;set; }
         public List<string> RequiredDependencies {get;set;}
         public ScriptType TypeOfScript{get;set;}
    }
}
