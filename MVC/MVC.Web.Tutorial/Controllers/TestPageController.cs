using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Web.Tutorial.Models;

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
        public ActionResult TestForm(TestFormModel model)
        {
            // accept params and do something with them
            // ie. insert into db

            // if input is valid
            if (ModelState.IsValid)
            {
                // do what you need to do
                // insertData(model.Name, model.Email, model.Phone);
                // then redirect user to a different screen so they can't repost same info
                return RedirectToAction("Index", "Test");
            }

            // do this if not valid
            return View();

            // BREAK: Up to 20:53
        }
    }
}