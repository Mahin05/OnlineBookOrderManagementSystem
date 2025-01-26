using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class CompanyReposiory : Repository<Company>, ICompanyReposiory
    {
        private readonly ApplicationDBContext _db;
        public CompanyReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }


        public async Task Update(Company entity)
        {
            _db.Companies.Update(entity);
        }

    }
}
