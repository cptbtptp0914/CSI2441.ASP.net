using System.Web.Mvc;

namespace A2.University.Web.Controllers
{

    /// <summary>
    /// Home controller class.
    /// </summary>
    public class HomeController : Controller
    {

        /// <summary>
        /// Home/Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Only used for info about SQLite
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Default template ActionResult, not used.
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Default template ActionResult, not used.
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
    }
}