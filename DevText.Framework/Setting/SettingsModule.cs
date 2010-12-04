using System;
using System.Reflection;
using Castle.Core.Interceptor;
using Module = Autofac.Module;

namespace DevText.Framework.Setting
{
    public class SettingsModule:Module
    {

    }

    public interface ISettingsModuleInterceptor : IInterceptor
    {       
    }

    public class SettingsModuleInterceptor:ISettingsModuleInterceptor
    {
        private readonly ISiteService _siteService;

        public SettingsModuleInterceptor(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.ReturnType == typeof(ISite) && invocation.Method.Name == "get_CurrentSite")
            {
                invocation.ReturnValue = _siteService.GetSiteSettings();
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
