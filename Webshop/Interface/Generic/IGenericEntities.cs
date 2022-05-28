using Webshop.Models.Base;
using Webshop.Models.JWT;

namespace Webshop.Interface.Generic
{
    public interface IUserEntities
    {
        public Guid Id { get; set; }


        //JWT
        public List<RefreshToken>? RefreshTokens { get; set; }
    }

    public interface IProductEntities
    {
        public Guid Id { get; set; }
        public ProductCategory Category { get; set; }
        public string ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public DateTime ProductCreated { get; set; }
    }
}
