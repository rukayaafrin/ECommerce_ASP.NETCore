﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Layout.Models;
using System.ComponentModel.Design;
using Layout.Db;

namespace Layout.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database db;

        public HomeController(Database db)
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
            ViewData["Is_Home"] = "menu_highlight";

            // use sessionId to determine if user has already logged in
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View();
        }
  
        [HttpPost]
        public IActionResult Search(string keyword)
        {
            return View();
        }
    }
}
