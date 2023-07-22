using BusinessObject;
using DataAccess.DTO;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CinemaDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(CinemaDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync();

        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[]? includes)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.ToListAsync();
        }


        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }



        //public Task<T> GetOneAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        //{
        //    throw new NotImplementedException();
        //}



        public async Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            await SaveAsync();
        }
    }
}
