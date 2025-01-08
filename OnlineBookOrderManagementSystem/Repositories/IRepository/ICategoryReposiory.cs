using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface ICategoryReposiory : IRepository<Category>
    {
        Task Save();
        Task Update(Category obj);
    }
}
