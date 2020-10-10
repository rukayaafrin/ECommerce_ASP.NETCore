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
using System.Data.Common;

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
            //with search keyword
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

            //no search keyword, display all products
            else
            {
                List<Product> products = db.Products.ToList();
                ViewData["products"] = products;

            }

            

            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);
            ViewData["numberOfProductsInCart"] = "0";
            
            //first expression evaluates that 
            //second expression evaluates that it's a existing guest user
            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
                //logged in user
                if (session != null)
                {
                    ViewData["username"] = session.User.Username;

                    Cart existingCart = db.Carts.FirstOrDefault(x => (x.UserId == session.UserId));

                    if (existingCart != null)
                    {
                        ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();
                    }

                }

                //guest user
                else
                {
                    Cart existingCart = db.Carts.FirstOrDefault(x => (x.UserId == guestUser.Id));

                    if (existingCart != null)
                    {
                        ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();
                    }
                }
            }

            // to highlight "Office" as the selected menu-item
            ViewData["Is_Home"] = "menu_highlight";
            ViewData["sessionId"] = Request.Cookies["sessionId"];

            return View();
        }

        [HttpPost]
        public IActionResult UpdateCart([FromBody] UpdateCartInput input)
        {
            Session session = db.Sessions.FirstOrDefault(x => x.Id == Request.Cookies["sessionId"]);
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);

            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            { 
                if (session != null)
                {
                    //retrieve existing cart from db
                    Cart existingCart = db.Carts.FirstOrDefault(x => (x.UserId == session.UserId));

                    //no existing cart; create new cart for user
                    if (existingCart == null)
                    {
                        Cart newCart = new Cart
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = session.UserId
                        };
                        db.Add(newCart);
                        db.SaveChanges();

                        CartDetail newCartDetail = new CartDetail
                        {
                            CartId = newCart.Id,
                            ProductId = input.ProductId,
                            UserId = session.UserId,
                            Quantity = 1
                        };
                        db.Add(newCartDetail);
                        db.SaveChanges();

                        //count number of products in cart 
                        ViewData["numberOfProductsInCart"] = newCart.CartDetails.ToList().Count();

                        Debug.WriteLine($"A new order {newCart.Id} has been created. 1 product {input.ProductId} has been added to the order details.");
                    }

                    //cart exists, retrieve existing cart details
                    else
                    {
                        List<CartDetail> existingCartDetails = existingCart.CartDetails.ToList();

                        //check if existing cart already has selected product
                        CartDetail cartDetailWithThisProduct = existingCartDetails.Find(x => x.ProductId == input.ProductId);

                        //if selected product does not exist in cart, create new cartdetail
                        if (cartDetailWithThisProduct == null)
                        {
                            CartDetail newCartDetail = new CartDetail
                            {
                                CartId = existingCart.Id,
                                ProductId = input.ProductId,
                                UserId = session.UserId,
                                Quantity = 1
                            };
                            db.Add(newCartDetail);
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();

                            Debug.WriteLine($"1 product {input.ProductId} has been added to the order details in existing order {existingCart.Id}.");
                        }
                        
                        //selected product already exists, increase qty by 1
                        else
                        {
                            cartDetailWithThisProduct.Quantity = cartDetailWithThisProduct.Quantity + 1;
                            db.SaveChanges();

                            //update no. of products in cart
                            ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();
                        }
                    }
                }

                else
                {
                    Cart existingCart = db.Carts.FirstOrDefault(x => (x.UserId == guestUser.Id));
                    if (existingCart == null)
                    {
                        Cart newCart = new Cart
                        {
                            Id = Guid.NewGuid().ToString(),
                            UserId = guestUser.Id
                        };
                        db.Add(newCart);
                        db.SaveChanges();

                        CartDetail newCartDetail = new CartDetail
                        {
                            CartId = newCart.Id,
                            ProductId = input.ProductId,
                            UserId = guestUser.Id,
                            Quantity = 1
                        };
                        db.Add(newCartDetail);
                        db.SaveChanges();

                        ViewData["numberOfProductsInCart"] = newCart.CartDetails.ToList().Count();

                        Debug.WriteLine($"A new order {newCart.Id} has been created. 1 product {input.ProductId} has been added to the cart details.");
                    }
                    else
                    {
                        List<CartDetail> existingCartDetails = existingCart.CartDetails.ToList();

                        CartDetail cartDetailWithThisProduct = existingCartDetails.Find(x => x.ProductId == input.ProductId);

                        if (cartDetailWithThisProduct == null)
                        {
                            CartDetail newCartDetail = new CartDetail
                            {
                                CartId = existingCart.Id,
                                ProductId = input.ProductId,
                                UserId = guestUser.Id,
                                Quantity = 1
                            };
                            db.Add(newCartDetail);
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();

                            Debug.WriteLine($"1 product {input.ProductId} has been added to the order details in existing cart {existingCart.Id}.");
                        }
                        else
                        {
                            cartDetailWithThisProduct.Quantity = cartDetailWithThisProduct.Quantity + 1;
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingCart.CartDetails.ToList().Count();
                        }
                    }
                }
            }
            return Json(new { status = "success", cartNumber = ViewData["numberOfProductsInCart"].ToString() });
        }
    }
}
