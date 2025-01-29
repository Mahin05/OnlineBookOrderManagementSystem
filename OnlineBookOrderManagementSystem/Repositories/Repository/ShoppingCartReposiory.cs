using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class ShoppingCartReposiory : Repository<ShoppingCart>, IShoppingCartReposiory
    {
        private readonly ApplicationDBContext _db;
        public ShoppingCartReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(ShoppingCart entity)
        {
            _db.ShoppingCarts.Update(entity);
        }

    }
}
