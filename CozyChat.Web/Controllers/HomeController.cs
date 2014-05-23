using System.Threading.Tasks;
using System.Web.Mvc;
using CozyChat.Web.Extensions;
using CozyChat.Web.Repositories;
using Microsoft.AspNet.Identity;

namespace CozyChat.Web.Controllers
{
    public class HomeController : AsyncController
    {
        private readonly ICozyChatRepository _repo;

        public HomeController(ICozyChatRepository repo)
        {
            _repo = repo;
        }

        [Authorize]
        public ActionResult ManageChatRooms()
        {
            //NOTE: SignalR can be used even in controller
            //var a = GlobalHost.ConnectionManager.GetHubContext<CozyChatHub>();
            //a.Clients.All.Notify("trololo");
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
            var res = await _repo.GetChatRoomsAsync();
            return new JsonNetResult { Data = res };
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetSubscribedRooms()
        {
            var res = await _repo.GetSubscribedChatRoomsAsync(int.Parse(User.Identity.GetUserId()));
            return new JsonNetResult {Data = res};
        }

        [Authorize, HttpGet]
        public async Task<JsonNetResult> GetMessagesForRoom(int userId, int roomId)
        {
            var res = await _repo.GetMessagesForChatRoomAsync(userId, roomId);
            return new JsonNetResult { Data = res };
        }

        [Authorize, HttpPost]
        public async Task<JsonNetResult> CreateChatRoom(string roomName)
        {

            var room = await _repo.CreateChatRoomAsync(int.Parse(User.Identity.GetUserId()), roomName);
            return new JsonNetResult { Data = room };
        }

        [Authorize, HttpDelete]
        public async Task<bool> DeleteChatRoom(int roomId)
        {
            var succ = await _repo.DeleteChatRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }

        [Authorize, HttpPost]
        public async Task<bool> Subscribe(int roomId)
        {
            var succ = await _repo.SubscribeUserForRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }

        [Authorize, HttpPost]
        public async Task<bool> UnSubscribe(int roomId)
        {
            var succ = await _repo.UnSubscribeUserFromRoomAsync(int.Parse(User.Identity.GetUserId()), roomId);
            return succ;
        }
    }
}