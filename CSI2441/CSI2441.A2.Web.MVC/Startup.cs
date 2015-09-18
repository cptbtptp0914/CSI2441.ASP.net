using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSI2441.A2.Web.MVC.Startup))]
namespace CSI2441.A2.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
