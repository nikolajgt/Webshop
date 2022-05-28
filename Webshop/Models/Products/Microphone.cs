using Webshop.Interface.Generic;
using Webshop.Models.Base;

namespace Webshop.Models.Products
{
    public class Microphone : IProductEntities
    {
        public Guid Id { get; set; }
        public ProductCategory Category { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }

        public DateTime ProductCreated { get; set; }

        //Empty constructor to initialize
        public Microphone() { }

        //Create product constructor
        public Microphone(ProductCategory category, string name, double price, int quantity)
        {
            Id = Guid.NewGuid();
            Category = category;
            ProductName = name;
            ProductPrice = price;
            ProductQuantity = quantity;
            ProductCreated = DateTime.UtcNow;
        }

        //Transfer from model 2 model
        public Microphone(Guid id, ProductCategory category, string name, double price, int quantity)
        {
            Id = id;
            Category = category;
            ProductName = name;
            ProductPrice = price;
            ProductQuantity = quantity;
            ProductCreated = DateTime.UtcNow;
        }
    }
}
