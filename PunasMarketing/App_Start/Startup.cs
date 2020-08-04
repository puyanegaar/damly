using Microsoft.Owin;
using Owin;
using PunasMarketing.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace PunasMarketing.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}