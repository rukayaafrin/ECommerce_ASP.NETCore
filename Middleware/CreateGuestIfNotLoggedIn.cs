using Layout.Db;
using Layout.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Middleware
{
    public class CreateGuestIfNotLoggedIn
    {
        private readonly RequestDelegate next;
        public CreateGuestIfNotLoggedIn(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, [FromServices] Database db)
        {
            //KIV this conditional if (context.Request.Method == "POST")
            string sessionId = context.Request.Cookies["sessionId"];
            string currentGuestId = context.Request.Cookies["guestId"];

            if (sessionId == null && currentGuestId == null)
            {
                User guest = new User { Id = Guid.NewGuid().ToString(), IsBlacklisted = false };
                db.Add(guest);
                db.SaveChanges();
                context.Response.Cookies.Append("guestId", guest.Id);
            }
            await next(context);
        }
    }
}
