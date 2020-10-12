using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class CartController : Controller
    {
        private readonly Database db;

        public CartController(Database db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);

            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
                Cart existingCart = null;
                //logged in user
                if (session != null)
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == session.UserId));
                    ViewData["username"] = session.User.Username;
                }
                //guest user
                else
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == guestUser.Id));

                }

                //cart exists
                if (existingCart != null)
                {
                    List<CartDetail> cartDetails = db.CartDetails.Where(x => x.CartId == existingCart.Id).ToList(); 
                    ViewData["cart"] = cartDetails;

                }

            }
            
            return View();
        }

        //for implementing ajax...
        public JsonResult GetData()
        {
            List<CartDetail> orderDetails = db.CartDetails.ToList();
            return Json(orderDetails);


        }


    }
}
