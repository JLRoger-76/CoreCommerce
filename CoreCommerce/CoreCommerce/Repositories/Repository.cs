using CoreCommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CoreCommerce.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        public Context _context = null;
        private DbSet<T> table = null;
        public GenericRepository(Context _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllIncludes(params Expression<Func<T, Object>>[] includes)
        {
            IQueryable<T> query = table.Include(includes[0]);
            foreach (var include in includes.Skip(1))
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> GetManyByIdIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table.Where(predicate).Include(includes[0]);
            foreach (var include in includes.Skip(1))
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await table.ToListAsync();
        }
        public async Task<T> GetByIdIncludes(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = table.Where(predicate).Include(includes[0]);
            foreach (var include in includes.Skip(1))
            {
                query = query.Include(include);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await table.FindAsync(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(T obj)
        {           
            table.Remove(obj);
        }
        public void Save()
        {
             _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }    
    }
}
