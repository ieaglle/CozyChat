using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CozyChat.Web.Startup))]
namespace CozyChat.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
