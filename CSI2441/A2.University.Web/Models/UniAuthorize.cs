using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A2.University.Web.Models
{

    /// <summary>
    /// Custom Authorize attrbute, redirects unauth user to Home Index.
    /// </summary>
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
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
        }
    }
}