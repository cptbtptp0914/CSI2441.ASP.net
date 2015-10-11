using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models;

namespace A2.University.Web.Controllers
{
    [UniAuthorize(Roles = "STAFF")]
    public class StaffPortalController : Controller
    {
        // GET: StaffPortal
        public ActionResult Index()
        {
            return View();
        }
    }
}