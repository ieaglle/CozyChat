using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CozyChat.Web.CozyChatServiceProxy;
using CozyChat.Web.Extensions;
using CozyChat.Web.Models;
using Microsoft.AspNet.Identity;

namespace CozyChat.Web.Controllers
{
    public class HomeController : AsyncController
    {
        readonly CozyChatServiceClient _proxy = new CozyChatServiceClient();

        private readonly Func<ChatRoom, ChatRoomModel> _converter = r => new ChatRoomModel
        {
            Id = r.Id,
            CreatorId = r.CreatorId,
            Creator = r.Creator != null ? r.Creator.Name : null,
            Name = r.Name,
            CreatedDate = r.CreatedDate,
            Users = r.Users.Select(s => s.Id),
        }; 

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Authorize]
        public ActionResult PrivateMessages()
        {
            return View();
        }

        [Authorize]
        public ActionResult ManageChatRooms()
        {
            return View();
        }

        [Authorize]
        public ActionResult Room(int id)
        {
            var a = id;
            return View();
        }

        [Authorize]
        public ActionResult Rooms()
        {
            return View();
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetAllChatRooms()
        {
            var res = await _proxy.GetChatRoomsAsync();
            return new JsonNetResult { Data = res.Select(_converter) };
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetSubscribedRooms()
        {
            var res = await _proxy.GetSubscribedChatRoomsAsync(int.Parse(User.Identity.GetUserId()));
            return new JsonNetResult {Data = res.Select(_converter)};
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetUsersSubscribedToRoom(int roomId)
        {
            var res = await _proxy.GetUsersSubscribedToChatRoomAsync(roomId);
            return new JsonNetResult {Data = res};
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetMessagesForRoom(int roomId)
        {
            var res = await _proxy.GetMessagesForChatRoomAsync(roomId);
            return new JsonNetResult {Data = res};
        }
            
        [Authorize,HttpPost]
        public async Task<JsonNetResult> CreateChatRoom(string roomName)
        {
            
            var room = await _proxy.CreateChatRoomAsync(int.Parse(User.Identity.GetUserId()), roomName);
            return new JsonNetResult {Data = _converter(room)};
        }

        [Authorize, HttpDelete]
        public async Task<bool> DeleteChatRoom(int roomId)
        {
            var succ = await _proxy.DeleteChatRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }

        [Authorize, HttpPost]
        public async Task<bool> Subscribe(int roomId)
        {
            var succ = await _proxy.SubscribeUserForRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }

        [Authorize, HttpPost]
        public async Task<bool> UnSubscribe(int roomId)
        {
            var succ = await _proxy.UnSubscribeUserForRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }
    }
}