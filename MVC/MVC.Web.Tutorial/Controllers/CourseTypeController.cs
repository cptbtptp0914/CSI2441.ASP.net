using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Web.Tutorial.Models;

namespace MVC.Web.Tutorial.Controllers
{
    public class CourseTypeController : Controller
    {
        // GET: CourseType
        public ActionResult Details(int id)
        {
            CourseTypeContext courseTypeContext = new CourseTypeContext();
            CourseType courseType = courseTypeContext.CourseTypes.Single(ct => ct.CourseID == id);

            return View(courseType);
        }
    }
}