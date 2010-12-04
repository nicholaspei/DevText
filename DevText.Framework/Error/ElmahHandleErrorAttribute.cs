using System;
using System.Web;
using System.Web.Mvc;

using Elmah;

namespace DevText.Web
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class ElmahHandleErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            Exception e = filterContext.Exception;

            if (!filterContext.ExceptionHandled || RaiseErrorSignal(e) || IsFiltered(filterContext))
            {
                return;
            }

            LogException(e);
        }

        private static bool RaiseErrorSignal(Exception e)
        {
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                return false;
            }

            ErrorSignal signal = ErrorSignal.FromContext(context);

            if (signal == null)
            {
                return false;
            }

            signal.Raise(e, context);

            return true;
        }

        private static bool IsFiltered(ExceptionContext context)
        {
            ErrorFilterConfiguration config = context.HttpContext.GetSection("elmah/errorFilter") as ErrorFilterConfiguration;

            if (config == null)
            {
                return false;
            }

            ErrorFilterModule.AssertionHelperContext testContext = new ErrorFilterModule.AssertionHelperContext(context.Exception, HttpContext.Current);

            return config.Assertion.Test(testContext);
        }

        private static void LogException(Exception e)
        {
            HttpContext context = HttpContext.Current;

            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }
    }
}
