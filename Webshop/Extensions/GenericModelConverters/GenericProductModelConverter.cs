
using Webshop.Interface.Generic;
using Webshop.Models.Base;

namespace Webshop.Extensions.GenericModelConverters
{
    public static class GenericProductModelConverter<TEntity> where TEntity : class, IProductEntities
    {
        public static async Task<List<Product>> GenericListConvertToBaseProduct(List<TEntity> entities)
        {
            List<Product> products = new List<Product>();
            foreach (TEntity entity in entities)
            {
                products.Add(new Product(entity.Id, entity.Category, entity.ProductName, entity.ProductPrice, entity.ProductQuantity));
            }
            return products;
        }

        public static async Task<Product> GenericListConvertToBaseProduct(TEntity entity)
        {
            return new Product(entity.Id, entity.Category, entity.ProductName, entity.ProductPrice, entity.ProductQuantity);
        }
    }
}
