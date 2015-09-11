using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Web.Tutorial.Models;

namespace MVC.Web.Tutorial.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            // can pass data dynamically with ViewData
            ViewData["helloWorld"] = "ViewData message";
            // experiment with ViewBag
            ViewBag.Message = "ViewBag message";
            // note: don't shadow VieData and ViewBag names, ie "Message",
            // will cause both to be updated

            // passing data from model
            var model = new TestModel();
            model.ModelMessage = "Model message";
            // pass model to view
            return View(model);
        }
    }
}