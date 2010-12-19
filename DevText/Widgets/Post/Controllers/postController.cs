using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevText.Framework.Mvc;
using DevText.Framework.Data;
using Post.Model;
using Post.Mapping;
using Post.Repository;


namespace Post.Controllers
{
    public class postController : NicholasController
    {
        private readonly IpostRepository postService;
        //
        // GET: /post/
        
       
        public ActionResult Index()
        {
       //     List<post> posts = postService.All().ToList();
          //  return View(posts);
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(post post)
        {
            postService.Add(post);

            return RedirectToAction("Index");
        }

        public ActionResult Install()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Install(string check)
        {
            SessionFactory CreateSession = new SessionFactory();
            CreateSession.CreateSessionFactory<PostMapping>();

            return RedirectToAction("Create");

        }
        }
}
