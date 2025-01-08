using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _db;
        public ICategoryReposiory Category { get; private set; }
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Category= new CategoryReposiory(_db);
        }

        public void Save()
        {
             _db.SaveChangesAsync();
        }

    }
}
