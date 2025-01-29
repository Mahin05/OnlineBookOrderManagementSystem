using OnlineBookOrderManagementSystem.Areas.Admin.Controllers;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Repositories.Repository
{
    public class ApplicationUserReposiory : Repository<ApplicationUser>, IApplicationUserReposiory
    {
        private readonly ApplicationDBContext _db;
        public ApplicationUserReposiory(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

    }
}
