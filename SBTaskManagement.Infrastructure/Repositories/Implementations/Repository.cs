using Microsoft.EntityFrameworkCore;
using SBTaskManagement.Infrastructure.DBContext;
using SBTaskManagement.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        internal DbSet<T> dbset;
        public Repository(AppDbContext context)
        {
            _context = context;
            dbset = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await dbset.AddAsync(entity);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().AnyAsync(predicate);
        }
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbset.CountAsync(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbset.Where(predicate).ToListAsync();
        }

        public Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return dbset.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbset.ToListAsync();
        }

        
        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await dbset.FindAsync(id);
            if (entity == null) return null;
            return entity;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression)
        {
            return dbset.AsQueryable().Where(expression);
        }

        public void Remove(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            dbset.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public void Update(T entity)
        {
            dbset.Update(entity);
        }
    }
}
