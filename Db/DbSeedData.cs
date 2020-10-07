using Layout.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Layout.Db
{
    public class DbSeedData
    {
        private readonly Database db;
        public DbSeedData(Database db)
        {
            this.db = db;
        }
        public void Seed()
        {
            AddUsers();
            AddBlacklistedUsers();
            AddProducts("SeedData/product.data");
        }
        public void AddUsers()
        {
            string[] usernameArr = new string[] { "Jon", "Jane" };
            string[] passwordArr = new string[] { "1234", "4321" };

            for (int i = 0; i < usernameArr.Length; i++)
            {
                db.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = usernameArr[i],
                    Password = passwordArr[i],
                    IsBlacklisted = false
                });
            }
            db.SaveChanges();
        }
        public void AddBlacklistedUsers()
        {
            string[] usernameArr = new string[] { "badboy", "gangsta" };
            string[] passwordArr = new string[] { "badboy", "gangsta" };

            for (int i = 0; i < usernameArr.Length; i++)
            {
                db.Add(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = usernameArr[i],
                    Password = passwordArr[i],
                    IsBlacklisted = true
                });
            }
            db.SaveChanges();
        }
        public void AddProducts(string filename)
        {
 
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                string[] quartet  = line.Split(";");
                if (quartet.Length == 4)
                {
                    db.Products.Add(new Product
                    {
                        Name = quartet[0],
                        Description = quartet[1],
                        Price = int.Parse(quartet[2]),
                        Image = quartet[3]
                    });
                }
            }

            db.SaveChanges();
        }
    }
}
