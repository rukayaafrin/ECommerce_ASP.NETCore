using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Layout.Models;

namespace Layout.DBContext
{

        public class DbProductGallery : DbContext
        {
            protected IConfiguration configuration;

            public DbProductGallery(DbContextOptions<DbProductGallery> options) : base(options)
            {

            }

            protected override void OnModelCreating(ModelBuilder model)
            {
                model.Entity<Products>().HasIndex(x => x.Id).IsUnique();
            }


            public DbSet<Products> products { get; set; }


        }
    
}
