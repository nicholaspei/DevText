using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web.Mvc;

namespace DevText.Framework.Theme
{
    public class ThemeManager :IThemeManager
    {
        private DirectoryInfo _rootDirectory;

        public ThemeManager(DirectoryInfo rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public IEnumerable<SelectListItem> Themes(string selectedvalue)
        {
            foreach (var theme in _rootDirectory.EnumerateDirectories())
            {
                yield return new SelectListItem
                {
                    Selected = (theme.Name == selectedvalue),
                    Text = theme.Name,
                    Value = theme.Name
                };
            }
        }
    }
}
