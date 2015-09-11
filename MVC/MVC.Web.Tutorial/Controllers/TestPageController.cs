using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Web.Tutorial.Controllers
{
    public class TestPageController : Controller
    {
        // always add get tag for readability
        [HttpGet]
        public ActionResult TestForm()
        {
            return View();
        }

        // define same method with [HttpPost] tag
        [HttpPost]
        // overload method with param
        public ActionResult TestForm(string name)
        {
            // accept params and do something with them
            // ie. insert into db

            // then redirect user to a different screen so they can't repost same info
            return RedirectToAction("Index", "Home");

            // BREAK: at 14:07 of video
        }
    }
}