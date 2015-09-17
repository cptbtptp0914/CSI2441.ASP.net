using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Exercises.WebForm.Startup))]
namespace Exercises.WebForm
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
