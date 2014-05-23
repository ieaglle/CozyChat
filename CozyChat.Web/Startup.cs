using CozyChat.Web.Extensions;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
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

            GlobalHost.DependencyResolver
                .Register(typeof (IHubActivator), () => new MvcHubActivator());

            app.MapSignalR();
        }
    }
}
