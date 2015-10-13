using System;
using System.Web.Mvc;

namespace A2.University.Web.Controllers
{

    /// <summary>
    /// Controller for StaffPortal
    /// </summary>
    public class StaffPortalController : Controller
    {
        /// <summary>
        /// GET: StaffPortal/Index
        /// Shows home page for StaffPortal. No dynamic data.
        /// See StaffPortal/Index view for defined links to each section.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}