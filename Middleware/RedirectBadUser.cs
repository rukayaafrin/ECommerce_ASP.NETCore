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
    public class RedirectBadUser
    {
        private readonly RequestDelegate next;
        public RedirectBadUser(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, [FromServices] Database db)
        {
            List<User> blacklist = db.Users.Where(x => x.IsBlacklisted).ToList();

            if (context.Request.Method == "POST")
            {
                string path = context.Request.Path;
                if (path.StartsWith("/Login/Authenticate"))
                {
                    //the line below collects the form inputs using a IFormCollection interface type
                    IFormCollection form = await context.Request.ReadFormAsync();
                    //the key of the form below is given by the name of the input on the client side.
                    string username = form["username"];
                    if (blacklist.Find(x => x.Username == username) != null)
                    {
                        context.Response.Redirect("/Login/NoEntry");
                        return;
                    }
                }
            }

            await next(context);
        }
    }
}
