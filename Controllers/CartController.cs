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
            List<CartDetail> orderDetails = db.CartDetails.ToList();
            ViewData["cart"] = orderDetails;



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
