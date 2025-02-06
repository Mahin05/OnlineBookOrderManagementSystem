using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class OrderHeaderReposiory : Repository<OrderHeader>, IOrderHeaderReposiory
    {
        private readonly ApplicationDBContext _db;
        public OrderHeaderReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(OrderHeader entity)
        {
            _db.OrderHeaders.Update(entity);
        }

    }
}
