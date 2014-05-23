using System.Collections.Generic;
using System.Threading.Tasks;
using CozyChat.Model;
using CozyChat.Web.Models;

namespace CozyChat.Web.Repositories
{
    public interface ICozyChatRepository
    {
        Task<User> CheckLoginAsync(string userName, string password);
        Task<User> RegisterUserAsync(string userName, string password);
        Task<IEnumerable<ChatRoomModel>> GetChatRoomsAsync();
        Task<IEnumerable<ChatRoomModel>> GetSubscribedChatRoomsAsync(int userId);
        Task<IEnumerable<MessageModel>> GetMessagesForChatRoomAsync(int userId, int roomId);
        Task<ChatRoomModel> CreateChatRoomAsync(int userId, string roomName);
        Task<bool> DeleteChatRoomAsync(int userId, int roomId);
        Task<bool> SubscribeUserForRoomAsync(int userId, int roomId);
        Task<bool> UnSubscribeUserFromRoomAsync(int userId, int roomId);
        Task<MessageModel> SendMessageAsync(int senderId, string content, int? receiverId, int? chatRoomId);
        Task<User> GetUserByNameAsync(string userName);
    }
}