using System;

namespace DevText.Framework.Setting
{
    public class CurrentSiteWorkContext
    {
        private readonly ISiteService _siteService;

        public CurrentSiteWorkContext(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public Func<T> Get<T>(string name)
        {
            if (name == "CurrentSite")
            {
                var siteSettings = _siteService.GetSiteSettings();
                return () => (T)siteSettings;
            }
            return null;
        }
    }
}
