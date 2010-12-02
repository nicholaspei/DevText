using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DevText.Framework.Theme
{
    public interface IThemeManager
    {
        IEnumerable<SelectListItem> Themes(string selectedvalue);
    }
}
