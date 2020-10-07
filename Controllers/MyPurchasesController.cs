using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class MyPurchasesController : Controller
    {
        private readonly Database db;

        public MyPurchasesController(Database db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            if (session != null)
            {
                ViewData["username"] = session.User.Username;
            }

            // to highlight "Office" as the selected menu-item
            ViewData["Is_MyPurchases"] = "menu_highlight";

            // use sessionId to determine if user has already logged in
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View();
        }
    }
}
