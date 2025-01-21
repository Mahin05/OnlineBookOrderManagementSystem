using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork= unitOfWork;
        }

        public IActionResult Index()
        {
            var data = _unitOfWork.Product.GetAll().Include(x=>x.Category).ToList();
            return View(data);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = _unitOfWork.Product.GetAll().Include(x=>x.Category).Where(x=>x.Id==id).FirstOrDefault();
            var products = _unitOfWork.Product.Get(x=>x.Id==id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
