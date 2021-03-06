﻿using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using CozyChat.Model;

namespace CozyChat.Service
{
    [ServiceContract]
    public interface ICozyChatService
    {
        #region ChatRoom Management

        [OperationContract]
        Task<List<ChatRoom>> GetChatRoomsAsync();

        [OperationContract]
        Task<ChatRoom> CreateChatRoomAsync(int userId, string chatRoomName);

        [OperationContract]
        Task<bool> DeleteChatRoomAsync(int userId, int chatRoomId);

        [OperationContract]
        Task<bool> SubscribeUserForRoomAsync(int userId, int roomId);

        [OperationContract]
        Task<bool> UnSubscribeUserForRoomAsync(int userId, int roomId);

        [OperationContract]
        Task<List<ChatRoom>> GetSubscribedChatRoomsAsync(int userId);

        [OperationContract]
        Task<List<User>> GetUsersSubscribedToChatRoomAsync(int chatRoomId);

        [OperationContract]
        Task<List<Message>> GetMessagesForChatRoomAsync(int userId, int chatRoomId);

        #endregion

        #region User Management

        [OperationContract]
        Task<User> RegisterUserAsync(string name, string password);

        [OperationContract]
        Task<User> CheckLoginAsync(string name, string password);

        [OperationContract]
        Task<User> GetUserByIdAsync(int userId);

        [OperationContract]
        Task<User> GetUserByNameAsync(string name);

        #endregion

        [OperationContract]
        Task<Message> SendMessageAsync(int senderId, string content, int? userId, int? chatRoomId);
    }
}