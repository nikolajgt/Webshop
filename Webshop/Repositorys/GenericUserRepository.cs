
using Microsoft.EntityFrameworkCore;
using Webshop.Interface.Generic;
using Webshop.Models;

namespace Webshop.Repositorys
{
    public class GenericUserRepository<TEntity> : IGenericUserRepository<TEntity>
                    where TEntity : class, IUserEntities
    {
        private readonly MyDbContext _db;
        private readonly ILogger _logger;
        private DbSet<TEntity> _dbSet;
        public GenericUserRepository(MyDbContext repo, ILogger logger)
        {
            _db = repo;
            _logger = logger;
            _dbSet = _db.Set<TEntity>();

        }

        public async Task<bool> PostUserEntityAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Post User method error", typeof(GenericUserRepository<TEntity>));
                return false;
            }
        }


        public async Task<bool> UpdateUserEntityAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Update User method error", typeof(GenericUserRepository<TEntity>));
                return false;
            }
        }

        public async Task<bool> UpdateUserEntityListAsync(List<TEntity> entity)
        {
            try
            {
                _dbSet.UpdateRange(entity);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Update User List method error", typeof(GenericUserRepository<TEntity>));
                return false;
            }
        }



        public async Task<TEntity> GetUserEntityByIDAsync(Guid stringid)
        {
            try
            {
                return await _dbSet.FindAsync(stringid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Get User method error", typeof(GenericUserRepository<TEntity>));
                return null;
            }
        }

        public async Task<bool> DeleteUserEntityAsync(Guid guid)
        {
            try
            {
                var exist = await _dbSet.Where(x => x.Id == guid).FirstOrDefaultAsync();

                if (exist != null)
                {
                    _dbSet.Remove(exist);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Delete User method error", typeof(GenericUserRepository<TEntity>));
                return false;
            }
        }

        public async Task<List<TEntity>> GetAllUserEntitiesAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Delete method error", typeof(GenericUserRepository<TEntity>));
                return null;
            }
        }


        // LOGIN

        //public async Task<TEntity> UserEntityLoginAsync(string username, string password)
        //{
        //    try
        //    {
        //        return await _db.Set<TEntity>().FirstOrDefaultAsync(x => x.UserName == username && x.Password == password);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}

        public async Task<TEntity> RefreshTokenUserEntity(string token)
        {
            try
            {
                return await _db.Set<TEntity>().FirstOrDefaultAsync(x => x.RefreshTokens.Any(y => y.Token == token));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
