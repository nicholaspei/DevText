using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevText.Framework.Data;

namespace DevText.Framework.Mvc
{
    public class DevTextControllerFactory: DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null) return base.GetControllerInstance(requestContext, controllerType);

            var args = new object[] { "post" };

            return Activator.CreateInstance(controllerType, args ) as IController;
        }
    }
}
