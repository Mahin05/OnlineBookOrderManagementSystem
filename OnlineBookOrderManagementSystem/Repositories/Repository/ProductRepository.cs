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

            var objFromDB = _db.products.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDB != null)
            {
                objFromDB.Title = entity.Title;
                objFromDB.ISBN = entity.ISBN;
                objFromDB.Price = entity.Price;
                objFromDB.Price50 = entity.Price50;
                objFromDB.Price100 = entity.Price100;
                objFromDB.ListPrice = entity.ListPrice;
                objFromDB.Discription = entity.Discription;
                objFromDB.CategoryId = entity.CategoryId;
                objFromDB.Author = entity.Author;
                if (entity.ImageUrl != null)
                {
                    objFromDB.ImageUrl = entity.ImageUrl;
                }
            }

            _db.products.Update(entity);
        }

    }
}
