using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

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

                string currentGuestId = Request.Cookies["guestId"];
                User currentGuestUser = db.Users.FirstOrDefault(x => x.Id == currentGuestId);

                Order orderMadeByGuest = db.Orders.FirstOrDefault(x => x.UserId == currentGuestUser.Id);

                if (orderMadeByGuest != null)
                {
                    List<OrderDetail> orderDetailsMadeByGuest = orderMadeByGuest.OrderDetails.ToList();

                    Order existingUnpaidOrder = db.Orders.FirstOrDefault(x => x.UserId == user.Id && x.PaidFor == false);
                    if (existingUnpaidOrder == null)
                    {
                        orderMadeByGuest.UserId = user.Id;

                        foreach (OrderDetail od in orderDetailsMadeByGuest)
                        {
                            od.UserId = user.Id;
                        }
                    }
                    else
                    {
                        List<OrderDetail> orderDetailsOfExistingUnpaidOrder = existingUnpaidOrder.OrderDetails.ToList();
                        foreach (OrderDetail od1 in orderDetailsOfExistingUnpaidOrder)
                        {
                            foreach (OrderDetail od2 in orderDetailsMadeByGuest)
                            {
                                if(od1.ProductId == od2.ProductId)
                                {
                                    od1.Quantity = od1.Quantity + od2.Quantity;
                                    db.Remove(od2);
                                    db.SaveChanges();
                                    break;
                                }
                            }
                        }
                        List<OrderDetail> remainingOrderDetailsMadeByGuest = orderMadeByGuest.OrderDetails.ToList();
                        foreach (OrderDetail od in remainingOrderDetailsMadeByGuest)
                        {
                            int tempProductId = od.ProductId;
                            int tempQuantity = od.Quantity;
                            db.Remove(od);
                            db.SaveChanges();
                            OrderDetail temp = new OrderDetail
                            {
                                OrderId = existingUnpaidOrder.Id,
                                ProductId = tempProductId,
                                Quantity = tempQuantity,
                                UserId = user.Id
                            };
                            db.Add(temp);
                            db.SaveChanges();
                        }
                        db.Remove(orderMadeByGuest);
                    }
                }

                db.Remove(currentGuestUser);
                db.SaveChanges();

                Response.Cookies.Delete("guestId");

                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult NoEntry()
        {
            return View();
        }
    }
}
