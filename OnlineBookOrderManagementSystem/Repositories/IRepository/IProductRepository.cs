using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IProductReposiory : IRepository<Product>
    {
        Task Update(Product obj);
    }
}
