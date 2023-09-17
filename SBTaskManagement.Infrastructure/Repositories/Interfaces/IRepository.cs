using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SBTaskManagement.Infrastructure.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        //Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAll();
        Task<T> GetByIdAsync(Guid Id);
        Task AddAsync(T entity);
        void Remove(T entity);
        void Update(T entity);
        Task<bool> SaveChangesAsync();
        Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query(Expression<Func<T, bool>> expression);
        bool Exists(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
       


    }
}
