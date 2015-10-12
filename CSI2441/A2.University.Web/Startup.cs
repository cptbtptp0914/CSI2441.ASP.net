using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(A2.University.Web.Startup))]
namespace A2.University.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
//            ConfigureAuth(app);
        }
    }
}
