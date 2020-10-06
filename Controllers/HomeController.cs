using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Layout.Models;

namespace Layout.Controllers
{
    public class HomeController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult MyPurchases()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Search(string keyword)
        {
            return View();
        }
    }
}
