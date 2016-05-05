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

        public ActionResult GeneralChat(string name)
        {
            // Require authentication if name is not set
            if (name == null)
                return View("Authenticate");

            ViewBag.Name = name;

            Models.ChatRoom model = new Models.ChatRoom();
            model.ID = Guid.NewGuid().ToString();
            model.Title = "Let's chat!";

            return View(model);
        }

        public ActionResult SupportChat(string name)
        {
            // Require authentication if name is not set
            if (name == null)
                return View("Authenticate");

            ViewBag.Name = name;

            Models.ChatRoom model = new Models.ChatRoom();
            model.ID = Guid.NewGuid().ToString();
            model.Title = "Support chat!";

            return View(model);
        }

        public ActionResult SupportAgent(string name)
        {
            // Require authentication if name is not set
            if (name == null)
                return View("Authenticate");

            ViewBag.Name = name;

            Models.ChatRoom model = new Models.ChatRoom();
            model.ID = Guid.NewGuid().ToString();
            model.Title = "Let's provide support!";

            return View(model);
        }

    }
}