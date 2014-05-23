// Oleksandr Babii
// 18/05/2014 1:25 
// 

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyChat.Model;
using CozyChat.Web.Models;
using CozyChat.Web.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;

namespace CozyChat.Web.SignalR
{
    [Authorize]
    public class CozyChatHub : Hub
    {
        private readonly ICozyChatRepository _repo;
        private static readonly ConcurrentDictionary<string, List<User>> OnlineUsers = new ConcurrentDictionary<string, List<User>>();

        public CozyChatHub(ICozyChatRepository repo)
        {
            _repo = repo;
        }

        public async Task SendToGroup(ChatRoomModel room, string text)
        {
            var isPm = text.StartsWith("pm(", StringComparison.InvariantCultureIgnoreCase);

            if (isPm)
            {
                var start = text.IndexOf('(');
                var end = text.IndexOf(')');
                var usrName = text.Substring(start + 1, end-start-1);
                var content = text.Substring(end+2);

                var usr = await _repo.GetUserByNameAsync(usrName);
                if (usr == null)
                {
                    Clients.Caller.MessageSent(new MessageModel
                    {
                        Sent = DateTime.Now, 
                        Sender = Context.User.Identity.Name, 
                        Content = string.Format("User \"{0}\" wasn't found", usrName)
                    });
                    return;
                }
                else
                {
                    var msg = await _repo.SendMessageAsync(int.Parse(Context.User.Identity.GetUserId()), content, usr.Id, room.Id);
                    Clients.User(usrName).MessageSent(msg);
                    Clients.Caller.MessageSent(msg);
                    return;
                }
            }

            var res = await _repo.SendMessageAsync(int.Parse(Context.User.Identity.GetUserId()), text, null, room.Id);
            Clients.Group(room.Name).MessageSent(res);
        }

        public async Task Join(ChatRoomModel group)
        {
            var userName = Context.User.Identity.Name;

            await Groups.Add(Context.ConnectionId, group.Name);
            var user = await _repo.GetUserByNameAsync(userName);

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
        }
    }
}