using System;
using System.Web.Mvc;

namespace A2.University.Web.Controllers
{

    /// <summary>
    /// Custom Authorize attrbute, redirects unauth user to Home Index.
    /// Deprecated, no longer using Identity Entity Framework.
    /// </summary>
    [Obsolete]
    public class UniAuthorize : AuthorizeAttribute
    {
        
        /// <summary>
        /// Overriding Authorize to redirect user to Home Index if unauthorized.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // if authorized
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                // call super
                base.OnAuthorization(filterContext);
            }
            else
            {
                // custom redirect
                filterContext.Result = new RedirectResult("~/StudentAccount/Login");
            }
        }
    }
}