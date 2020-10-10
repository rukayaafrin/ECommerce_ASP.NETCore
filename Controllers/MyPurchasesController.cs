using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            //redirect user back to login page if not logged in
            if (session.User.Username == null)
            {
                return RedirectToAction("Index", "Login");
            }

            //retrieve list of past purchases
            List<PurchaseDetail> purchases = db.PurchaseDetails.Where(x => x.UserId == session.UserId).ToList();

            ViewData["purchases"] = purchases;


            /*//generate unique list of products purchased
            List<Product> pdtspurchased = null; 
            foreach (var purchase in purchases)
            {
                Product product = db.Products.Where(x => x.Id == purchase.ProductId).FirstOrDefault();
                if (!pdtspurchased.Contains(product))
                {
                    pdtspurchased.Add(product);
                }
            }
            ViewData["pdtspurchased"] = pdtspurchased;*/


            //to display username and correct nav bar
            ViewData["sessionId"] = session.Id;
            ViewData["username"] = session.User.Username;
            // to highlight "Office" as the selected menu-item
            ViewData["Is_MyPurchases"] = "menu_highlight";

            return View();
        }
    }
}
