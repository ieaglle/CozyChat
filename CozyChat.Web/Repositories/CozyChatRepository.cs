using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CozyChat.Model;
using CozyChat.Web.Extensions;
using CozyChat.Web.Models;

namespace CozyChat.Web.Repositories
{
    public class CozyChatRepository : ICozyChatRepository
    {
        private readonly Func<ChatRoom, ChatRoomModel> _converter = r => new ChatRoomModel
        {
            Id = r.Id,
            CreatorId = r.CreatorId,
            Creator = r.Creator != null ? r.Creator.Name : null,
            Name = r.Name,
            CreatedDate = r.CreatedDate,
            Users = r.Users.Select(s => s.Id),
        };

        private readonly Func<Message, MessageModel> _messagesConverter = m => new MessageModel
        {
            Id = m.Id,
            Content = m.Content,
            Sender = m.Sender.Name,
            Sent = m.SentDate
        };

        public async Task<User> CheckLoginAsync(string userName, string password)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.CheckLoginAsync(userName, password);
            }
        }

        public async Task<User> RegisterUserAsync(string userName, string password)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.RegisterUserAsync(userName, password);
            }
        }

        public async Task<IEnumerable<ChatRoomModel>> GetChatRoomsAsync()
        {
            using (var proxy = new CozyChatProxy())
            {
                var rooms = await proxy.Channel.GetChatRoomsAsync();
                return rooms.Select(s => _converter(s));
            }
        }

        public async Task<IEnumerable<ChatRoomModel>> GetSubscribedChatRoomsAsync(int userId)
        {
            using (var proxy = new CozyChatProxy())
            {
                var rooms = await proxy.Channel.GetSubscribedChatRoomsAsync(userId);
                return rooms.Select(s => _converter(s));
            }
        }

        public async Task<IEnumerable<MessageModel>> GetMessagesForChatRoomAsync(int userId, int roomId)
        {
            using (var proxy = new CozyChatProxy())
            {
                var msgs = await proxy.Channel.GetMessagesForChatRoomAsync(userId, roomId);
                return msgs.Select(s => _messagesConverter(s));
            }
        }

        public async Task<ChatRoomModel> CreateChatRoomAsync(int userId, string roomName)
        {
            using (var proxy = new CozyChatProxy())
            {
                var room = await proxy.Channel.CreateChatRoomAsync(userId, roomName);
                return _converter(room);
            }
        }

        public async Task<bool> DeleteChatRoomAsync(int userId, int roomId)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.DeleteChatRoomAsync(userId, roomId);
            }
        }

        public async Task<bool> SubscribeUserForRoomAsync(int userId, int roomId)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.SubscribeUserForRoomAsync(userId, roomId);
            }
        }

        public async Task<bool> UnSubscribeUserFromRoomAsync(int userId, int roomId)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.UnSubscribeUserForRoomAsync(userId, roomId);
            }
        }

        public async Task<MessageModel> SendMessageAsync(int senderId, string content, int? receiverId, int? chatRoomId)
        {
            using (var proxy = new CozyChatProxy())
            {
                var msg =  await proxy.Channel.SendMessageAsync(senderId, content, receiverId, chatRoomId);
                return _messagesConverter(msg);
            }
        }

        public async Task<User> GetUserByNameAsync(string userName)
        {
            using (var proxy = new CozyChatProxy())
            {
                return await proxy.Channel.GetUserByNameAsync(userName);
            }
        }
    }
}