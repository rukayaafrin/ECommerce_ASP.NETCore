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
            User guestUser = db.Users.FirstOrDefault(x => x.Id == Request.Cookies["guestId"]);
            ViewData["numberOfProductsInCart"] = "0";
            if ((session != null || guestUser != null) && !(session != null && guestUser != null))
            {
                if (session != null)
                {
                    ViewData["username"] = session.User.Username;

                    Order existingOrder = db.Orders.FirstOrDefault(x => (x.UserId == session.UserId) && (x.PaidFor == false));

                    if (existingOrder != null)
                    {
                        ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();
                    }

                }
                else
                {
                    Order existingOrder = db.Orders.FirstOrDefault(x => (x.UserId == guestUser.Id) && (x.PaidFor == false));

                    if (existingOrder != null)
                    {
                        ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();
                    }
                }
            }
            // to highlight "Office" as the selected menu-item
            ViewData["Is_Home"] = "menu_highlight";

            // use sessionId to determine if user has already logged in
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
                    Order existingOrder = db.Orders.FirstOrDefault(x => (x.UserId == session.UserId) && (x.PaidFor == false));

                    //conditional check for current user if he has an existing order that has not been paid for.
                    //if no, use the conditional below.
                    if (existingOrder == null)
                    {
                        Order newOrder = new Order
                        {
                            Id = Guid.NewGuid().ToString(),
                            PaidFor = false,
                            UserId = session.UserId
                        };
                        db.Add(newOrder);
                        db.SaveChanges();

                        OrderDetail newOrderDetail = new OrderDetail
                        {
                            OrderId = newOrder.Id,
                            ProductId = int.Parse(input.ProductId),
                            UserId = session.UserId,
                            Quantity = 1
                        };
                        db.Add(newOrderDetail);
                        db.SaveChanges();

                        ViewData["numberOfProductsInCart"] = newOrder.OrderDetails.ToList().Count();

                        Debug.WriteLine($"A new order {newOrder.Id} has been created. 1 product {input.ProductId} has been added to the order details.");
                    }
                    else
                    {
                        List<OrderDetail> existingOrderDetails = existingOrder.OrderDetails.ToList();

                        OrderDetail orderDetailWithThisProduct = existingOrderDetails.Find(x => x.ProductId == int.Parse(input.ProductId));

                        if (orderDetailWithThisProduct == null)
                        {
                            OrderDetail newOrderDetail = new OrderDetail
                            {
                                OrderId = existingOrder.Id,
                                ProductId = int.Parse(input.ProductId),
                                UserId = session.UserId,
                                Quantity = 1
                            };
                            db.Add(newOrderDetail);
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();

                            Debug.WriteLine($"1 product {input.ProductId} has been added to the order details in existing order {existingOrder.Id}.");
                        }
                        else
                        {
                            orderDetailWithThisProduct.Quantity = orderDetailWithThisProduct.Quantity + 1;
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();
                        }
                    }
                }
                else
                {
                    Order existingOrder = db.Orders.FirstOrDefault(x => (x.UserId == guestUser.Id) && (x.PaidFor == false));
                    if (existingOrder == null)
                    {
                        Order newOrder = new Order
                        {
                            Id = Guid.NewGuid().ToString(),
                            PaidFor = false,
                            UserId = guestUser.Id
                        };
                        db.Add(newOrder);
                        db.SaveChanges();

                        OrderDetail newOrderDetail = new OrderDetail
                        {
                            OrderId = newOrder.Id,
                            ProductId = int.Parse(input.ProductId),
                            UserId = guestUser.Id,
                            Quantity = 1
                        };
                        db.Add(newOrderDetail);
                        db.SaveChanges();

                        ViewData["numberOfProductsInCart"] = newOrder.OrderDetails.ToList().Count();

                        Debug.WriteLine($"A new order {newOrder.Id} has been created. 1 product {input.ProductId} has been added to the order details.");
                    }
                    else
                    {
                        List<OrderDetail> existingOrderDetails = existingOrder.OrderDetails.ToList();

                        OrderDetail orderDetailWithThisProduct = existingOrderDetails.Find(x => x.ProductId == int.Parse(input.ProductId));

                        if (orderDetailWithThisProduct == null)
                        {
                            OrderDetail newOrderDetail = new OrderDetail
                            {
                                OrderId = existingOrder.Id,
                                ProductId = int.Parse(input.ProductId),
                                UserId = guestUser.Id,
                                Quantity = 1
                            };
                            db.Add(newOrderDetail);
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();

                            Debug.WriteLine($"1 product {input.ProductId} has been added to the order details in existing order {existingOrder.Id}.");
                        }
                        else
                        {
                            orderDetailWithThisProduct.Quantity = orderDetailWithThisProduct.Quantity + 1;
                            db.SaveChanges();

                            ViewData["numberOfProductsInCart"] = existingOrder.OrderDetails.ToList().Count();
                        }
                    }
                }
            }
            return Json(new { status = "success", cartNumber = ViewData["numberOfProductsInCart"].ToString() });
        }
    }
}
