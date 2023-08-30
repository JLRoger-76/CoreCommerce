using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreCommerce.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllIncludes(params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> GetManyByIdIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetById(object id);
        Task<T> GetByIdIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);
        void Save();
        void Dispose();
    }

}
