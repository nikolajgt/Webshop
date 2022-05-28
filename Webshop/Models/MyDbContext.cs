
using Microsoft.EntityFrameworkCore;
using Webshop.Models.Products;
using Webshop.Models.Shopping;

namespace Webshop.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //User types
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }


        //Shopping
        public DbSet<CustomerBasket> CustomerBasket { get; set; }
        public DbSet<ProductOrders> Orders { get; set; }
        public DbSet<OrderShipping> Shipping { get; set; }


        //Product types
        public DbSet<Keyboard> Smartphones { get; set; }
        public DbSet<Smartphone> Keyboards { get; set; }
        public DbSet<Microphone> Microphones { get; set; }
        public DbSet<Mouse> Mouse { get; set; }
    }
}
