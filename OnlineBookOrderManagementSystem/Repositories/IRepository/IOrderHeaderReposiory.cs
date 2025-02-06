using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IOrderHeaderReposiory : IRepository<OrderHeader>
    {
        Task Update(OrderHeader entity);
    }
}
