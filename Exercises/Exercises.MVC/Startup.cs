using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exercises.MVC.Startup))]
namespace Exercises.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
