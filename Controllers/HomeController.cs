using System;
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

        public IActionResult Index(string keyword)
        {
            if (keyword != null)
            {
                ViewData["GetProductDetails"] = keyword;
                var prodquery = from x in db.Products select x;
                if (!string.IsNullOrEmpty(keyword))
                {
                    prodquery = prodquery.Where(x => x.Description.Contains(keyword) || x.Name.Contains(keyword));
                }

                ViewData["products"] = prodquery.ToList();
            }
            else
            {
                List<Product> products = db.Products.ToList();
                ViewData["products"] = products;

            }

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
    }
}
