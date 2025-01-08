using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface ICategoryReposiory : IRepository<Category>
    {
        Task Update(Category obj);
    }
}
