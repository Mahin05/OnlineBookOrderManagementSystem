using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface ICompanyReposiory : IRepository<Company>
    {
        Task Update(Company obj);
    }
}
