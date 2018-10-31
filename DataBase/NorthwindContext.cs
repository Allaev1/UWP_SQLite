using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class NorthwindContext : DbContext
    {
        public DbSet<Product> Products;

        protected override void OnConfiguring(DbContextOptionsBuilder dbContext)
        {
            dbContext.UseSqlite("Filename=NORTHWIND.sqlite");
        }
    }
}
