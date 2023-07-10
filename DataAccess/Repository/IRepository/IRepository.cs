using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //GetALL
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        //Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);

        //GetOne
        //Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task UpdateAsync(T entity);
        Task SaveAsync();

    }
}
