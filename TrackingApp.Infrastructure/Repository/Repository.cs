using TrackingApp.Core.Interface;
using TrackingApp.Core.Shared;
using TrackingApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LearningSystem.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly TAContext _dbContext;

        public Repository(TAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Insert(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            _dbContext.Set<T>().Add(entity);
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
                throw ex;
            }
            
        }

        public async Task<int> BulkInsert(List<T> entity)
        {
            entity.All(a => { a.CreatedDate = DateTime.UtcNow; return true; });
            _dbContext.Set<T>().AddRange(entity);
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Detached;
                throw ex;
            }
            
        }
        public async Task<int> BulkDelete(List<T> entity)
        {
            entity.All(a => { a.ModifiedDate = DateTime.UtcNow; a.IsDeleted = true; return true; });
            _dbContext.Set<T>().UpdateRange(entity);
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Unchanged;
                throw ex;
            }
        }
        public async Task<int> Delete(T entity)
        {
            // soft delete
            entity.ModifiedDate = DateTime.UtcNow;
            entity.IsDeleted = true;
            _dbContext.Entry(entity).State = EntityState.Modified;
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Unchanged;
                throw ex;
            }
            
        }

        public async Task<int> HardDelete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Unchanged;
                throw ex;
            }
        }

        public async Task<int> Update(T entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _dbContext.Entry(entity).State = EntityState.Unchanged;
                throw ex;
            }
        }

        public async virtual Task<T> GetById(int id, string include)
        {
            var entity = _dbContext.Set<T>().Where(a => a.Id == id && (a.IsDeleted != true || a.IsDeleted == null));
            if (!string.IsNullOrEmpty(include))
                entity.Include(include);
            return await entity.FirstOrDefaultAsync();
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return _dbContext.Set<T>().Where(a => a.IsDeleted != true || a.IsDeleted == null)
                //.Include(a => a.CreatedByName)
                //.Include(a => a.ModifiedByName)                
                .AsEnumerable();
        }

        public async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string include)
        {
            var list = _dbContext.Set<T>()
               .Where(predicate)
               .Where(a => a.IsDeleted != true || a.IsDeleted == null);
            if (!string.IsNullOrEmpty(include))
                list = list.Include(include);

            return list.AsEnumerable();
        }

        public async Task<T> Find(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
               .Where(predicate)
               .Where(a => a.IsDeleted != true || a.IsDeleted == null)
               .FirstOrDefaultAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return (await GetById(id, null) != null);
        }

        private void HandleSavingExceptions(List<T> entries)
        {
           
        }

    }
}
