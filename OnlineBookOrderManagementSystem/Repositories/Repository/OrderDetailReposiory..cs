using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class OrderDetailReposiory : Repository<OrderDetail>, IOrderDetailReposiory
    {
        private readonly ApplicationDBContext _db;
        public OrderDetailReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(OrderDetail entity)
        {
            _db.OrderDetails.Update(entity);
        }

    }
}
