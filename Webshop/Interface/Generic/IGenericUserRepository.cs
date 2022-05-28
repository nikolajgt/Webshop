namespace Webshop.Interface.Generic
{
    public interface IGenericUserRepository<TEntity>
    {
        Task<bool> PostUserEntityAsync(TEntity entity);

       
        Task<TEntity> GetUserEntityByIDAsync(Guid stringid);


        Task<bool> UpdateUserEntityListAsync(List<TEntity> entity);
        Task<bool> UpdateUserEntityAsync(TEntity entity);
        Task<bool> DeleteUserEntityAsync(Guid guid);

        Task<List<TEntity>> GetAllUserEntitiesAsync();

        //Task<TEntity> UserEntityLoginAsync(string username, string password);
        Task<TEntity> RefreshTokenUserEntity(string token);

    }
}
