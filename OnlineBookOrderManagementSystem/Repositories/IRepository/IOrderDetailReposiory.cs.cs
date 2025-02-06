using OnlineBookOrderManagementSystem.Models;

namespace OnlineBookOrderManagementSystem.Repositories.IRepository
{
    public interface IOrderDetailReposiory : IRepository<OrderDetail>
    {
        Task Update(OrderDetail entity);
    }
}
