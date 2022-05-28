
using Microsoft.EntityFrameworkCore;
using Webshop.Interface.Generic;
using Webshop.Models;

namespace Webshop.Repositorys
{
    public class GenericProductRepository<TEntity> : IGenericProductRepository<TEntity>
                    where TEntity : class, IProductEntities
    {

        private readonly MyDbContext _db;
        private readonly ILogger _logger;
        private DbSet<TEntity> _dbSet;
        public GenericProductRepository(MyDbContext repo, ILogger logger)
        {
            _db = repo;
            _logger = logger;
            _dbSet = _db.Set<TEntity>();

        }


        public async Task<bool> PostProductEntityAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Post Product method error", typeof(GenericProductRepository<TEntity>));
                return false;
            }
        }


        public async Task<bool> UpdateProductEntityAsync(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Update Product method error", typeof(GenericProductRepository<TEntity>));
                return false;
            }
        }

        public async Task<bool> UpdateProductEntityListAsync(List<TEntity> entity)
        {
            try
            {
                _dbSet.UpdateRange(entity);
                return (await _db.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Update Product List method error", typeof(GenericProductRepository<TEntity>));
                return false;
            }
        }



        public async Task<TEntity> GetProductEntityByIDAsync(Guid stringid)
        {
            try
            {
                return await _dbSet.FindAsync(stringid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Get Product method error", typeof(GenericProductRepository<TEntity>));
                return null;
            }
        }

        public async Task<bool> DeleteProductEntityAsync(Guid guid)
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
                _logger.LogError(ex, "{REPO} Delete method error", typeof(GenericProductRepository<TEntity>));
                return false;
            }
        }

        public async Task<List<TEntity>> GetAllProductEntitiesAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{REPO} Get All Products method error", typeof(GenericProductRepository<TEntity>));
                return null;
            }
        }
    }
}
