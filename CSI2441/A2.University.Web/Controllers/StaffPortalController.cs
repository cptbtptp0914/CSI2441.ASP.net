using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A2.University.Web.Controllers
{
    [Authorize(Roles = "STAFF")]
    public class StaffPortalController : Controller
    {
        // GET: StaffPortal
        public ActionResult Index()
        {
            return View();
        }
    }
}