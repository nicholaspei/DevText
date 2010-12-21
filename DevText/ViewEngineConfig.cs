using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcExtensions;
using System.Web.Mvc;
using System.Web.Routing;
using DevText.Framework.Mvc;

namespace DevText
{
    public class ViewEngineConfig:BootstrapperTask
    {
        public override TaskContinuation Execute()
        {
            // registor viewengine here
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new DevTextViewEngine());

           return  TaskContinuation.Continue;
        }
    }
}