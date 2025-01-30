using OnlineBookOrderManagementSystem.Areas.Customer.Controllers;
using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IShoppingCartReposiory : IRepository<ShoppingCart>
    {
        Task Update(ShoppingCart obj);
        //void Update(IQueryable<ShoppingCart> obj);
    }
}
