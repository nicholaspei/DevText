using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DevText.Framework.Widget;
using DevText.Framework.Theme;
using DevText.Framework.Email;
using DevText.Framework.File;
using DevText.Framework.Security;
using DevText.Framework.Data;
using DevText.Framework.Setting;
using DevText.Web;

namespace DevText.Framework.Mvc
{
    [ElmahHandleError]
    public class NicholasController:Controller
    {
        public IRepository<IUser> Repository;

        public NicholasController() { }
        public NicholasController(IRepository<IUser> repository)
        {
            Repository = repository;
        }
        public IUser CurrentUser
        { 
            get
            {
                //if (Request.IsAuthenticated)
                    return Repository.GetBy(x => x.UserName.Equals(User.Identity.Name)).FirstOrDefault();
              // will change the code here ,when the User part finish.
                //  else
            //        return new IUser { Id = 0, UserName = "anonymous" };
            }
        }

        private ThemeManager _themeManager;
        public ThemeManager ThemeManager
        {
            get
            {
                if (_themeManager == null)
                    _themeManager = new ThemeManager(FileManager.GetDirectory("../Themes"));

                return _themeManager;
            }
        }



        private EmailManager _emailManager;
        public EmailManager EmailManager
        {
            get
            {
                if (_emailManager == null)
                    _emailManager = new EmailManager(FileManager.EmailDirectory, Settings);

                return _emailManager;
            }
        }

        public ISite Settings;

        public string Widget;
      


    }
}
