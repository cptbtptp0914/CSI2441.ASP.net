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
            ViewData["message"] = "Hello World!";
            return View();
        }
    }
}