using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Layout.DBContext;
using Layout.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Layout
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddControllersWithViews();

            services.AddDbContext<DbProductGallery>(opt => opt.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("DbConn")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, DbProductGallery db)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            bool wantReset = Configuration.GetValue<bool>("Db:WantReset");
            if (wantReset)
            {
                db.Database.EnsureDeleted();    // wipe out existing database
                db.Database.EnsureCreated();    // our database is created after this line

                Products product1 = new Products
                {

                    Name = ".NET Charts",
                    Description = "Brings powerful charting capabilities to your .NET applications.",
                    Price = 99,
                    Image = "/images/Charts.png"
                }
                ;

                db.Add(product1);

                Products product2 = new Products
                {

                    Name = ".NET Paypal",
                    Description = "Integrate your .NET apps with PayPal the easy way!",
                    Price = 69,
                    Image = "/images/Paypal.png"
                }
               ;
                db.Add(product2);

                Products product3 = new Products
                {

                    Name = ".NET ML",
                    Description = "Supercharged .NET machine learning libraries.",
                    Price = 299,
                    Image = "/images/ML.png"
                }
                ;
                db.Add(product3);


                Products product4 = new Products
                {

                    Name = ".NET Analytics",
                    Description = "Perform data mining and analytics easily in .NET.",
                    Price = 299,
                    Image = "/images/Analytics.png"
                }
                ;
                db.Add(product4);


                Products product5 = new Products
                {

                    Name = ".NET Logger",
                    Description = "Logs and aggregates events easily in your .NET apps.",
                    Price = 49,
                    Image = "/images/Logger.png"
                }
                ;
                db.Add(product5);


                Products product6 = new Products
                {

                    Name = ".NET Numerics",
                    Description = "Brings powerful charting capabilities to your .NET applications.",
                    Price = 199,
                    Image = "/images/Numerics.png"
                }
                ;
                db.Add(product6);


                Products product7 = new Products
                {

                    Name = ".NET Photos",
                    Description = "Brings beautiful edits to your .NET application pictures.",
                    Price = 199,
                    Image = "/images/Photos.png"
                }
               ;
                db.Add(product7);

                db.SaveChanges();

            }
        }
    }
}
