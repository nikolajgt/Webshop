namespace Webshop.Interface.Generic
{
    public interface IGenericProductRepository<TEntity>
    {
        Task<bool> PostProductEntityAsync(TEntity entity);


        Task<TEntity> GetProductEntityByIDAsync(Guid stringid);


        Task<bool> UpdateProductEntityListAsync(List<TEntity> entity);
        Task<bool> UpdateProductEntityAsync(TEntity entity);
        Task<bool> DeleteProductEntityAsync(Guid guid);

        Task<List<TEntity>> GetAllProductEntitiesAsync();
    }
}
