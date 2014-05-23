using System.Web.Mvc;
using Microsoft.AspNet.SignalR.Hubs;

namespace CozyChat.Web.Extensions
{
    public class MvcHubActivator : IHubActivator
    {
        public IHub Create(HubDescriptor descriptor)
        {
            return (IHub) DependencyResolver.Current.GetService(descriptor.HubType);
        }
    }
}