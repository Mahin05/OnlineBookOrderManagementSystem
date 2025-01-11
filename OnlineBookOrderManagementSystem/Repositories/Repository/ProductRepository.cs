using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class ProductReposiory : Repository<Product>, IProductReposiory
    {
        private readonly ApplicationDBContext _db;
        public ProductReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(Product entity)
        {
            _db.Update(entity);
        }

    }
}
