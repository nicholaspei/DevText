using System;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections;
using System.Web.Hosting;
using System.Globalization;
using System.Linq;

namespace DevText.Framework.Mvc
{
   public  class DevTextViewEngine:WebFormViewEngine
    {

       public DevTextViewEngine()            
        {
            // Search paths for the master pages
            base.MasterLocationFormats = new string[] {
                "~/Themes/{2}/{0}.master", 
                "~/Themes/{2}/Views/Shared/{0}.master",
                "~/Views/Shared/{0}.master"                
            };
            // Search paths for the views
            base.ViewLocationFormats = new string[] { 
                "~/Themes/{2}/Views/{1}/{0}.aspx",                 
                "~/Themes/{2}/Views/Shared/{0}.aspx",
                "~/Themes/{2}/Views/{1}/{0}.ascx",                
                "~/Themes/{2}/Views/Shared/{0}.ascx",
                "~/Widgets/{1}/Views/{1}/{0}.aspx",                 
                "~/Widgets/{1}/Views/Shared/{0}.aspx",
                "~/Widgets/{1}/Views/{1}/{0}.ascx",                
                "~/Widgets/{1}/Views/Shared/{0}.ascx",
                "~/Views/{1}/{0}.aspx",
                "~/Views/Shared/{0}.aspx",     
                "~/Views/{1}/{0}.ascx",
                "~/Views/Shared/{0}.ascx"
            };
            // Search parts for the partial views
            // The search parts for the partial views are the same as the regular views
            base.PartialViewLocationFormats = base.ViewLocationFormats;
        }

        #region Helper Methods
        private static string GetTheme( ControllerContext controllerContext )
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                throw new InvalidOperationException("Http Context cannot be null.");
            }

            NicholasController controller = (NicholasController)controllerContext.Controller;
        //    return String.IsNullOrWhiteSpace(context.Request["theme"]) ? controller.Settings.Theme : context.Request["theme"];
            return "default";
        }

       private static string GetWidget(ControllerContext controllerContext)
        {

            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                throw new InvalidOperationException("Http Context cannot be null.");
            }

            NicholasController controller = (NicholasController)controllerContext.Controller;
           return controller.Widget;
        }

       

        private string GetPath(ControllerContext controllerContext, string[] locations, string name,
                                string theme, string controller, string cacheKeyPrefix, bool useCache, out string[] searchedLocations)
        {
            searchedLocations = new string[] { };

            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }
            if ((locations == null) || (locations.Length == 0))
            {
                throw new InvalidOperationException("locations must not be null or emtpy.");
            }

            bool flag = IsSpecificPath(name);
            string key = this.CreateCacheKey(cacheKeyPrefix, name, flag ? string.Empty : controller, theme);
            if (useCache)
            {
                string viewLocation = this.ViewLocationCache.GetViewLocation(controllerContext.HttpContext, key);
                if (viewLocation != null)
                {
                    return viewLocation;
                }
            }
            if (!flag)
            {
                string path = this.GetPathFromGeneralName(controllerContext, locations, name, controller, theme, key, ref searchedLocations);
                if (String.IsNullOrEmpty(path))
                {
                    path = this.GetPathFromGeneralName(controllerContext, locations, name, controller, "Default", key, ref searchedLocations);
                }
                return path;
            }
            return this.GetPathFromSpecificName(controllerContext, name, key, ref searchedLocations);
        }

        private static bool IsSpecificPath(string name)
        {
            char firstCharacter = name[0];
            if (firstCharacter != '~')
            {
                return (firstCharacter == '/');
            }
            return true;
        }

        private string CreateCacheKey(string prefix, string name, string controllerName, string theme)
        {
            return string.Format(CultureInfo.InvariantCulture, ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}",
                new object[] { base.GetType().AssemblyQualifiedName, prefix, name, controllerName, theme });
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, string[] locations, string name,
                                               string controller, string theme, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = string.Empty;
            searchedLocations = new string[locations.Length];
            for (int i = 0; i < locations.Length; i++)
            {
                string path = string.Format(CultureInfo.InvariantCulture, locations[i], new object[] { name, controller, theme });

                if (this.FileExists(controllerContext, path))
                {
                    searchedLocations = new string[] { };
                    virtualPath = path;
                    this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
                    return virtualPath;
                }
                searchedLocations[i] = path;
            }
            return virtualPath;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, ref string[] searchedLocations)
        {
            string virtualPath = name;

            if (!FileExists(controllerContext, name))
            {
                virtualPath = String.Empty;
                searchedLocations = new[] { name };
            }

            this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, virtualPath);
            return virtualPath;
        }
        #endregion

        #region Override Default Behavior
        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            try
            {
                return System.IO.File.Exists(controllerContext.HttpContext.Server.MapPath(virtualPath));
            }
            catch (HttpException exception)
            {
                if (exception.GetHttpCode() != 0x194)
                    throw;
                return false;
            }
            catch
            {
                return false;
            }
        }


        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(partialViewName))
            {
                throw new ArgumentException("partialViewName");
            }

            string[] partialViewLocationsSearched;
            string theme = GetTheme(controllerContext);
            string widget = GetWidget(controllerContext);

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string partialViewPath = this.GetPath(controllerContext, this.PartialViewLocationFormats, partialViewName, theme, controllerName, "Partial", useCache, out partialViewLocationsSearched); ;

            if (string.IsNullOrEmpty(partialViewPath))
            {
                partialViewPath = this.GetPath(controllerContext, this.PartialViewLocationFormats, partialViewName, widget, controllerName, "Partial", useCache, out partialViewLocationsSearched); ;    
               if(string.IsNullOrEmpty(partialViewPath))
                return new ViewEngineResult(partialViewLocationsSearched);
            }
            return new ViewEngineResult(this.CreatePartialView(controllerContext, partialViewPath), this);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }
            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;

            string theme = GetTheme(controllerContext);

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");
            string viewPath = this.GetPath(controllerContext, this.ViewLocationFormats, viewName,
                                       theme, controllerName, "View", useCache, out viewLocationsSearched);

            if (String.IsNullOrEmpty(masterName))
            {
                masterName = "Theme";
            }

            string masterPath = GetPath(controllerContext, MasterLocationFormats, masterName, theme, controllerName, "Master", useCache, out masterLocationsSearched);

            if (!string.IsNullOrEmpty(viewPath) && (!string.IsNullOrEmpty(masterPath) || string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(this.CreateView(controllerContext, viewPath, masterPath), this);
            }

            return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
        }
        #endregion        
    }
}
