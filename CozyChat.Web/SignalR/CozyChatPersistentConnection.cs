// Oleksandr Babii
// 18/05/2014 0:03 
// 

using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace CozyChat.Web.SignalR
{
    public class CozyChatPersistentConnection : PersistentConnection
    {
        #region Overrides of PersistentConnection

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var a = request.User;
            return base.OnConnected(request, connectionId);
        }

        #region Overrides of PersistentConnection

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            var a = request.User;
            return base.OnDisconnected(request, connectionId);
        }

        #endregion

        #endregion
    }
}