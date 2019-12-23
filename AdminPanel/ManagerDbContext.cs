using AdminPanel.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext() : base("Server=KVITLICH;Database=Products;Trusted_Connection=true;")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<Category> Categories { get; set; } 
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
