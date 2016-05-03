using System;
using System.Web.Mvc;

namespace ChatClient.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChatRoomAuthenticate(string name)
        {
            ViewBag.Name = name;

            Models.ChatRoom model = new Models.ChatRoom();
            model.ID = Guid.NewGuid().ToString();
            model.Title = "Let's chat!";

            return View("_PartialChatRoom", model);
        }

        public ActionResult ChatRoom()
        {
            return View();

        }
    }
}