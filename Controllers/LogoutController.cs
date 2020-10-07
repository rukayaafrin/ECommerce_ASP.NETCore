using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class LogoutController : Controller
    {
        private readonly Database db;

        public LogoutController(Database db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {

            // remove the user's sessionId from our record
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            Session currentSession = db.Sessions.FirstOrDefault(x => x.Id == sessionId);
            db.Remove(currentSession);
            db.SaveChanges();

            HttpContext.Response.Cookies.Delete("sessionId");

            // direct user back to our default gallery
            return RedirectToAction("Index", "Home");
        }
    }
}
