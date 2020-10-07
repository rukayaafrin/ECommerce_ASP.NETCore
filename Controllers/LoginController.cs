using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class LoginController : Controller
    {
        private readonly Database db;

        public LoginController(Database db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewData["Is_Login"] = "menu_highlight";
            ViewData["Title"] = "Login";
            return View();
        }
        public IActionResult Authenticate(string username, string password)
        {
            User user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

            if (user == null)
            {
                ViewData["lasttypedusername"] = username;
                ViewData["errMsg"] = "Incorrect username or password.";
                ViewData["Is_Login"] = "menu_highlight";
                return View("Index");
            }
            else
            {
                Session session = new Session()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Timestamp = DateTimeOffset.Now.ToUnixTimeSeconds()
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                Response.Cookies.Append("sessionId", session.Id);
                Response.Cookies.Append("username", username);
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult NoEntry()
        {
            return View();
        }
    }
}
