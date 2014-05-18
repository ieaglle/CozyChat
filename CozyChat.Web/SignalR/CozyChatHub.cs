// Oleksandr Babii
// 18/05/2014 1:25 
// 

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyChat.Web.CozyChatServiceProxy;
using CozyChat.Web.Models;
using Microsoft.AspNet.SignalR;

namespace CozyChat.Web.SignalR
{
    public class CozyChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<User>> OnlineUsers = new ConcurrentDictionary<string, List<User>>();
        private static readonly ConcurrentDictionary<string, List<string>> UserClients = new ConcurrentDictionary<string, List<string>>();

        readonly CozyChatServiceClient _proxy;

        public CozyChatHub()
        {
            _proxy = new CozyChatServiceClient();
        }

        public void SendToGroup(string group, string text)
        {

        }

        public async Task Join(ChatRoomModel group)
        {
            var userName = Context.User.Identity.Name;

            await Groups.Add(Context.ConnectionId, group.Name);
            var user = await _proxy.GetUserByNameAsync(userName);
            _proxy.Close();

            //creating group if it's not yet created
            if (!OnlineUsers.ContainsKey(group.Name))
                OnlineUsers.TryAdd(group.Name, new List<User>());

            //adding user if he's not yet in
            if (!OnlineUsers[group.Name].Select(s => s.Name).Contains(userName))
                OnlineUsers[group.Name].Add(user);

            //single user may have multiple clients (2 browser windows)
            //thus we need to manage user's clients

            //if user enters room for the first time
            if (!UserClients.ContainsKey(userName))
                UserClients.TryAdd(userName, new List<string>());

            Clients.Group(group.Name).UserJoined(OnlineUsers[group.Name].OrderBy(o => o.Name));
        }

        public void Leave(ChatRoomModel room)
        {
            Groups.Remove(Context.ConnectionId, room.Name);

            UserClients[Context.User.Identity.Name].Remove(Context.ConnectionId);

            OnlineUsers[room.Name].Remove(OnlineUsers[room.Name].First(f => f.Name == Context.User.Identity.Name));

            Clients.OthersInGroup(room.Name).UserLeft(OnlineUsers[room.Name].OrderBy(o => o.Name));
            _proxy.Close();
        }

        public void BrowserClosing(ChatRoomModel group)
        {
            
        }
    }
}