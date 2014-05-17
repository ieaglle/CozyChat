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
        private readonly CozyChatContext _context = new CozyChatContext();

        public Task<List<ChatRoom>> GetChatRoomsAsync()
        {
            return _context.ChatRooms
                .Include(i => i.Creator)
                .Include(i => i.Users)
                .Where(w => w.IsCurrent).ToListAsync();
        }

        public async Task<ChatRoom> CreateChatRoomAsync(int userId, string chatRoomName)
        {
            var check = await _context.ChatRooms.FirstOrDefaultAsync(s => s.Name == chatRoomName && s.IsCurrent);
            if (check != null)
                return null;

            _context.ChatRooms.Add(
                new ChatRoom
                {
                    CreatorId = userId,
                    Name = chatRoomName,
                    CreatedDate = DateTime.Now,
                    IsCurrent = true
                });
            await _context.SaveChangesAsync();

            return await _context.ChatRooms.Include(i => i.Creator).FirstAsync(f => f.Name == chatRoomName);
        }

        public async Task<bool> DeleteChatRoomAsync(int userId, int chatRoomId)
        {
            var check = await _context.ChatRooms.FirstOrDefaultAsync(f => f.Id == chatRoomId);

            if (check == null || check.CreatorId != userId)
                return false;

            check.IsCurrent = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SubscribeUserForRoomAsync(int userId, int roomId)
        {
            var group = await _context.ChatRooms.Include(i => i.Users).FirstOrDefaultAsync(f => f.Id == roomId);
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (group != null && user != null)
            {
                group.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UnSubscribeUserForRoomAsync(int userId, int roomId)
        {
            var group = await _context.ChatRooms.Include(i => i.Users).FirstOrDefaultAsync(f => f.Id == roomId);
            var user = await _context.Users.FirstOrDefaultAsync(f => f.Id == userId);
            if (group != null && user != null)
            {
                group.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<ChatRoom>> GetSubscribedChatRoomsAsync(int userId)
        {
            return await _context.ChatRooms.Where(w => w.Users.Any(u => u.Id == userId)).ToListAsync();
        }

        public async Task<List<User>> GetUsersSubscribedToChatRoomAsync(int chatRoomId)
        {
            return await _context.Users.Where(w => w.ChatRooms.Any(c => c.Id == chatRoomId)).ToListAsync();
        }

        public async Task<List<Message>> GetMessagesForChatRoomAsync(int chatRoomId)
        {
            return await _context.Messages.Where(w => w.ChatRoomId == chatRoomId).OrderBy(o => o.SentDate).ToListAsync();
        }

        public async Task<User> RegisterUserAsync(string name, string password)
        {
            var check = await _context.Users.FirstOrDefaultAsync(f => f.Name == name);

            if (check != null)
                return null;

            var usr = _context.Users.Add(
                new User
                {
                    Name = name,
                    Password = password,
                    RegisteredDate = DateTime.Now,
                    LastSeenDate = DateTime.Now
                });

            await _context.SaveChangesAsync();

            return usr;
        }

        public async Task<User> CheckLoginAsync(string name, string password)
        {
            var check = await _context.Users.FirstOrDefaultAsync(f => f.Name == name && f.Password == password);
            return check;
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

            _context.Messages.Add(message);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
