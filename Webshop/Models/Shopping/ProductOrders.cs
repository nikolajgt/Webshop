
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Webshop.Models.Base;

namespace Webshop.Models.Shopping
{
    public class ProductOrders
    {
        ProductOrders() { }

        [Key]
        public int OrderID { get; set; }
        public string? StreetName { get; set; }
        public string? StreetNumber { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public bool IsDelivered { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Product ItemOrdered{get; set; } 

        public ProductOrders(string streetname, string streetnumber, string city, string zipcode, Customer customer, Product item)
        {
            StreetName = streetname;
            StreetNumber = streetnumber;
            City = city;
            Zipcode = zipcode;
            Customer = customer;
            ItemOrdered = item;
        }
    }
}
