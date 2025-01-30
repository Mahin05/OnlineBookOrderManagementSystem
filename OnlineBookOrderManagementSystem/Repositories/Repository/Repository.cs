using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;
using System.Linq.Expressions;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbset;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbset = _db.Set<T>();
        }
        //public async Task Add(T entity)
        //{
        //    await dbset.AddAsync(entity);
        //}
        public void Add(T entity)
        {
            dbset.AddAsync(entity);
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public Task GetCart(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return query.FirstOrDefaultAsync();
        }
        public Task<T> GetDetails(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbset;
            query = query.Where(filter);
            return query.FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll()
        {
            //IQueryable<T> query = dbset;
            return dbset;
        }
        public IEnumerable<T> GetAllForDropDown()
        {
            IQueryable<T> query = dbset;
            return query.ToList();
        }

        public async Task Remove(T entity)
        {
            dbset.Remove(entity);
        }

        public async Task RemoveRange(T entity)
        {
            dbset.RemoveRange(entity);
        }

        public async Task Update(T entity)
        {
            dbset.Update(entity);
        }
        //public void Update(T entity)
        //{
        //    dbset.Update(entity);
        //}

    }
}
