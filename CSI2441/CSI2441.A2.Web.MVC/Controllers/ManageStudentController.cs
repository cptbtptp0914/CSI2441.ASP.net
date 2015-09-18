using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSI2441.A2.Web.MVC.Controllers
{
    public class ManageStudentController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        // overload Index() for [HttpPost]
    }
}