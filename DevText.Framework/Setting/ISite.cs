using System;

namespace DevText.Framework.Setting
{
    public interface ISite
    {
        string PageTitleSeparator { get; }
        string SiteName { get; }
        string SiteSalt { get; }
        string SuperUser { get; set; }
        string SiteUrl { get; set; }
        string Noreplayemail { get; set; }
        string SiteCulture { get; set; }
        string Theme { get; set; }
        ResourceDebugMode ResourceDebugMode { get; set; }
    }
}
