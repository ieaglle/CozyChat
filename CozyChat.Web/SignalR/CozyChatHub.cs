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
    [Authorize]
    public class CozyChatHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<User>> OnlineUsers = new ConcurrentDictionary<string, List<User>>();
        readonly CozyChatServiceClient _proxy;

        public CozyChatHub()
        {
            _proxy = new CozyChatServiceClient();
        }

        public async Task SendToGroup(ChatRoomModel room, string text)
        {
            //var res = await _proxy.SendMessageAsync(int.Parse(Context.User.Identity.GetUserId()), text, null, room.Id);
            Clients.Group(room.Name).MessageSent(new{content = text});
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

            Clients.Group(group.Name).UserJoined(OnlineUsers[group.Name].OrderBy(o => o.Name));
        }

        public void Leave(ChatRoomModel room)
        {
            if (room == null) return;

            if (!OnlineUsers.ContainsKey(room.Name) || 
                OnlineUsers[room.Name].All(u => u.Name != Context.User.Identity.Name)) 
                return;

            Groups.Remove(Context.ConnectionId, room.Name);

            OnlineUsers[room.Name].Remove(OnlineUsers[room.Name].First(f => f.Name == Context.User.Identity.Name));

            Clients.OthersInGroup(room.Name).UserLeft(OnlineUsers[room.Name].OrderBy(o => o.Name));
            _proxy.Close();
        }
    }
}