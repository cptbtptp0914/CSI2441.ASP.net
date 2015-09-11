using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC.Web.Tutorial.Startup))]
namespace MVC.Web.Tutorial
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
