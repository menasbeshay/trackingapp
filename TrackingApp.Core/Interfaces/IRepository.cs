using TrackingApp.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TrackingApp.Core.Interface
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetById(int id, string include);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> predicate, string include);
        Task<T> Find(Expression<Func<T, bool>> predicate);
        Task<int> Insert(T entity);
        Task<int> BulkInsert(List<T> entity);
        Task<int> BulkDelete(List<T> entity);
        Task<int> Delete(T entity);
        Task<int> HardDelete(T entity);
        Task<int> Update(T entity);
        Task<bool> Exists(int id);
    }
}
