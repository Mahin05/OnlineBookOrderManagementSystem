using BulkyBook.DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _db;
        public ICategoryReposiory Category { get; private set; }
        public IProductReposiory Product { get; private set; }
        public ICompanyReposiory Company { get; private set; }
        public IShoppingCartReposiory ShoppingCart { get; private set; }
        public IOrderHeaderReposiory OrderHeader { get; private set; }
        public IOrderDetailReposiory OrderDetail { get; private set; }
        public UnitOfWork(ApplicationDBContext db)
        {
            _db = db;
            Category= new CategoryReposiory(_db);
            Product= new ProductReposiory(_db);
            Company = new CompanyReposiory(_db);
            ShoppingCart = new ShoppingCartReposiory(_db);
            OrderHeader = new OrderHeaderReposiory(_db);
            OrderDetail = new OrderDetailReposiory(_db);
        }

        //public async Task Save()
        //{
        //     await _db.SaveChangesAsync();
        //}
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}
