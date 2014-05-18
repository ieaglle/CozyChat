using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CozyChat.Model;

namespace CozyChat.Service
{
    public class CozyChatService : ICozyChatService
    {
        public async Task<List<ChatRoom>> GetChatRoomsAsync()
        {
            List<ChatRoom> rooms;
            using (var ctx = new CozyChatContext())
            {
                rooms = await ctx.ChatRooms
                            .Include(i => i.Creator)
                            .Include(i => i.Users)
                            .Where(w => w.IsCurrent)
                            .ToListAsync();
            }
            return rooms;
        }

        public async Task<ChatRoom> CreateChatRoomAsync(int userId, string chatRoomName)
        {
            ChatRoom room;
            using (var ctx = new CozyChatContext())
            {
                var check = await ctx.ChatRooms.FirstOrDefaultAsync(s => s.Name == chatRoomName && s.IsCurrent);
                if (check != null)
                    return null;

                ctx.ChatRooms.Add(
                    new ChatRoom
                    {
                        CreatorId = userId,
                        Name = chatRoomName,
                        CreatedDate = DateTime.Now,
                        IsCurrent = true
                    });
                await ctx.SaveChangesAsync();

                room = await ctx.ChatRooms.Include(i => i.Creator).FirstAsync(f => f.Name == chatRoomName);
            }
            return room;
        }

        public async Task<bool> DeleteChatRoomAsync(int userId, int chatRoomId)
        {
            using (var ctx = new CozyChatContext())
            {
                var check = await ctx.ChatRooms.FirstOrDefaultAsync(f => f.Id == chatRoomId);

                if (check == null || check.CreatorId != userId)
                    return false;

                check.IsCurrent = false;
                await ctx.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> SubscribeUserForRoomAsync(int userId, int roomId)
        {
            using (var ctx = new CozyChatContext())
            {
                var group = await ctx.ChatRooms.Include(i => i.Users).FirstOrDefaultAsync(f => f.Id == roomId);
                var user = await ctx.Users.FirstOrDefaultAsync(f => f.Id == userId);
                if (group != null && user != null)
                {
                    group.Users.Add(user);
                    await ctx.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<bool> UnSubscribeUserForRoomAsync(int userId, int roomId)
        {
            using (var ctx = new CozyChatContext())
            {
                var group = await ctx.ChatRooms.Include(i => i.Users).FirstOrDefaultAsync(f => f.Id == roomId);
                var user = await ctx.Users.FirstOrDefaultAsync(f => f.Id == userId);
                if (group != null && user != null)
                {
                    group.Users.Remove(user);
                    await ctx.SaveChangesAsync();
                    return true;
                }
                return false;
            }
        }

        public async Task<List<ChatRoom>> GetSubscribedChatRoomsAsync(int userId)
        {
            List<ChatRoom> chatRooms;
            using (var ctx = new CozyChatContext())
            {
                chatRooms = await ctx.ChatRooms.Where(w => w.Users.Any(u => u.Id == userId)).ToListAsync();
            }
            return chatRooms;
        }

        public async Task<List<User>> GetUsersSubscribedToChatRoomAsync(int chatRoomId)
        {
            List<User> users;
            using (var ctx = new CozyChatContext())
            {
                users = await ctx.Users.Where(w => w.ChatRooms.Any(c => c.Id == chatRoomId)).ToListAsync();
            }
            return users;
        }

        public async Task<List<Message>> GetMessagesForChatRoomAsync(int chatRoomId)
        {
            List<Message> messages;
            using (var ctx = new CozyChatContext())
            {
                messages = await ctx.Messages.Where(w => w.ChatRoomId == chatRoomId).OrderBy(o => o.SentDate).ToListAsync();
            }
            return messages;
        }

        public async Task<User> RegisterUserAsync(string name, string password)
        {
            User user;
            using (var ctx = new CozyChatContext())
            {
                var check = await ctx.Users.FirstOrDefaultAsync(f => f.Name == name);

                if (check != null)
                    return null;

                user = ctx.Users.Add(
                    new User
                    {
                        Name = name,
                        Password = password,
                        RegisteredDate = DateTime.Now,
                        LastSeenDate = DateTime.Now
                    });

                await ctx.SaveChangesAsync();
            }
            return user;
        }

        public async Task<User> CheckLoginAsync(string name, string password)
        {
            User user;
            using (var ctx = new CozyChatContext())
            {
                user = await ctx.Users.FirstOrDefaultAsync(f => f.Name == name && f.Password == password);
            }
            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            User user;
            using (var ctx = new CozyChatContext())
            {
                user = await ctx.Users.FirstOrDefaultAsync(f => f.Id == userId);
            }
            return user;
        }

        public async Task<User> GetUserByNameAsync(string name)
        {
            User user;
            using (var ctx = new CozyChatContext())
            {
                user = await ctx.Users.FirstOrDefaultAsync(f => f.Name == name);
            }
            return user;
        }

        public async Task<bool> SendMessageAsync(int senderId, string content, int? userId, int? chatRoomId)
        {
            var message = new Message
            {
                Content = content,
                SenderId = senderId,
                ReceiverId = userId,
                ChatRoomId = chatRoomId,
                SentDate = DateTime.Now,
                IsRead = false
            };

            using (var ctx = new CozyChatContext())
            {
                ctx.Messages.Add(message);

                await ctx.SaveChangesAsync();
            }
            return true;
        }
    }
}
