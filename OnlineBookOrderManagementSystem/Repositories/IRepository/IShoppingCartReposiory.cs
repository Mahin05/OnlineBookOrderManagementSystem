using OnlineBookOrderManagementSystem.Areas.Customer.Controllers;
using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IShoppingCartReposiory : IRepository<ShoppingCart>
    {
        void Update(ShoppingCart entity);
        //void Update(IQueryable<ShoppingCart> obj);
    }
}
