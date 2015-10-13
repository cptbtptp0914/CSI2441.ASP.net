using System;
using System.Web.Mvc;

namespace A2.University.Web.Controllers
{

    /// <summary>
    /// Shows index of StaffPortal
    /// </summary>
    public class StaffPortalController : Controller
    {
        // GET: StaffPortal
        public ActionResult Index()
        {
            return View();
        }
    }
}