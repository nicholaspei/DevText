using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MvcExtensions.Autofac;
using Autofac.Integration.Web;

using Elmah;

namespace DevText
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : AutofacMvcApplication
    {
        public void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            var exception = e.Exception.GetBaseException() as HttpException;

            if ((exception != null) && (exception.GetHttpCode() == (int)HttpStatusCode.NotFound))
            {
                e.Dismiss();
            }
        }

       
    }
}