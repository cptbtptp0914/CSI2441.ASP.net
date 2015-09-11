using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC.Web.Tutorial
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            // call own RegisterRoutes instead of static
            RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // create new route
            routes.MapRoute(
                // the controller
                "Test",
                // which folder you want to be at URL,  "" == no folder
                "",
                // create new Test controller and the action to call
                new {controller = "Test", action = "Index"}    
            );

            // route for TestForm
            routes.MapRoute(
                "TestPage",
                "testpage",
                new {controller = "TestPage", action = "TestForm"}
            );
        }
    }
}
