using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevText.Framework.Mvc;

namespace Post.Controllers
{
    public class postController : NicholasController
    {
        //
        // GET: /post/
        public postController()
        {
            base.Widget = "Post";
        }

       
        public ActionResult Index()
        {
            return View();
        }

    }
}
