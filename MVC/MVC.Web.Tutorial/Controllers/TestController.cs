using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Web.Tutorial.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            // can pass data dynamically with ViewData
            ViewData["helloWorld"] = "Hello World!";
            // experiment with ViewBag
            ViewBag.Message = "ViewBag say's hi!";
            // note: don't shadow VieData and ViewBag names, ie "Message",
            // will cause both to be updated
            return View();
        }
    }
}