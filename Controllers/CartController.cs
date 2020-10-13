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
            Cart existingCart = null;
            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
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

        [HttpPost]
        public IActionResult UpdateQuantityInCart([FromBody] UpdateQuantityInput input)
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);
            Cart existingCart = null;
            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
                //logged in user
                if (session != null)
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == session.UserId));
                }
                //guest user
                else
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == guestUser.Id));

                }
            }
            List<CartDetail> existingCartDetails = existingCart.CartDetails.ToList();
            CartDetail cartDetailWithThisProduct = existingCartDetails.Find(x => x.ProductId == int.Parse(input.ProductId));

            if (input.Plus)
            {
                cartDetailWithThisProduct.Quantity = cartDetailWithThisProduct.Quantity + 1;
                db.SaveChanges();
            }
            else
            {
                if(cartDetailWithThisProduct.Quantity > 1)
                {
                    cartDetailWithThisProduct.Quantity = cartDetailWithThisProduct.Quantity - 1;
                    db.SaveChanges();
                }
            }

            int TotalPrice = 0;
            foreach (CartDetail cd in existingCartDetails)
            {
                TotalPrice = TotalPrice + cd.Quantity * cd.Product.Price;
            }

            return Json(new { status = "success", productId = input.ProductId, price = cartDetailWithThisProduct.Product.Price, quantity = cartDetailWithThisProduct.Quantity, totalprice = TotalPrice });
        }
        public IActionResult Remove(string productId)
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);
            Cart existingCart = null;
            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
                //logged in user
                if (session != null)
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == session.UserId));
                }
                //guest user
                else
                {
                    existingCart = db.Carts.FirstOrDefault(x => (x.UserId == guestUser.Id));

                }
            }
            List<CartDetail> existingCartDetails = existingCart.CartDetails.ToList();
            CartDetail cartDetailWithThisProduct = existingCartDetails.Find(x => x.ProductId == int.Parse(productId));
            db.Remove(cartDetailWithThisProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
