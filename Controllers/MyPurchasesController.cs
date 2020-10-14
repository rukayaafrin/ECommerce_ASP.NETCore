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
            if (session == null)
            {
                return RedirectToAction("Index", "Login");
            }


            //retrieve list of past purchases ordered by purchase date
            List<PurchaseDetail> purchases = db.PurchaseDetails.Where(x => x.UserId == session.UserId)
                                                               .OrderBy(x=> x.Purchase.Timestamp).ToList();

            //if purchases exist
            if (purchases.Count()>0)
            {
                //get list of pdtid of unique products from the purchases
                List<int> pdtIds = new List<int> { purchases[0].ProductId };

                for (int i = 1; i < purchases.Count(); i++)
                {
                    if (!pdtIds.Contains(purchases[i].ProductId))
                    {
                        pdtIds.Add(purchases[i].ProductId);
                    }
                }

                //group purchasedetails by pdt
                List<PurGroupedByPdt> grpPurchases = new List<PurGroupedByPdt>();
                foreach (int pdtId in pdtIds)
                {
                    //get list of purchasedetails with same pdtId 
                    List<PurchaseDetail> pcWithSamePdtId = (from pc in purchases
                                                            where pc.ProductId == pdtId
                                                            select pc).ToList();
                    //get total qty of items with same pdtId from different purchases
                    int totalquantity = 0;
                    foreach (PurchaseDetail pc in pcWithSamePdtId)
                    {
                        totalquantity += pc.Quantity;
                    }

                    PurGroupedByPdt currentgrp = new PurGroupedByPdt
                    {
                        ProductId = pdtId,
                        PurchasedItems = pcWithSamePdtId,
                        TotalQuantity = totalquantity
                    };

                    grpPurchases.Add(currentgrp);
                }


                ViewData["grpPurchases"] = grpPurchases;

            }
                

            //to display username and correct nav bar
            ViewData["username"] = session.User.Username;
            // to highlight "Office" as the selected menu-item
            ViewData["Is_MyPurchases"] = "menu_highlight";

            return View();
        }

        [HttpPost]
        public IActionResult GetDate([FromBody] AtvKeyInput input)
        {
            ActivationKey ak = db.ActivationKeys.Where(x => x.PdtAtvKey == input.AtvKey).FirstOrDefault();
            PurchaseDetail p = db.PurchaseDetails.Where(x => x.PurchaseId == ak.PurchaseDetailPurchaseId 
                                                && x.ProductId == ak.PurchaseDetailProductId).FirstOrDefault();
            var dateFormat = "dd MMM yyyy";
            DateTimeOffset pdatetime = DateTimeOffset.FromUnixTimeSeconds(p.Purchase.Timestamp).ToLocalTime();
            string pdate = pdatetime.ToString(dateFormat);
            return Json(new {pdate, pdtId=p.ProductId});
        }
    }
}
