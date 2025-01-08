using OnlineBookOrderManagementSystem.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class CategoryReposiory : Repository<Category>, ICategoryReposiory
    {
        private readonly ApplicationDBContext _db;
        public CategoryReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(CategoryController entity)
        {
            _db.Update(entity);
        }

    }
}
