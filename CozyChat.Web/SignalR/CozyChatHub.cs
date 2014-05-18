// Oleksandr Babii
// 18/05/2014 1:25 
// 

using Microsoft.AspNet.SignalR;

namespace CozyChat.Web.SignalR
{
    public class CozyChatHub : Hub
    {
        public void SendToGroup(string group, string text)
        {

        }

        public void Join(string group)
        {
            var usr = Context.User.Identity.Name;
            Clients.Others.Notify(group);
        }

        public void Leave(string group)
        {
            
        }
    }
}