using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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

        public int Count { get; private set; }

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
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //var product = _unitOfWork.Product.GetAll().Include(x=>x.Category).Where(x=>x.Id==id).FirstOrDefault();
            //var products = _unitOfWork.Product.Get(x=>x.Id==id);

            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.GetAll().Include(x => x.Category).Where(x => x.Id == id).FirstOrDefault(),
                Count = 1,
                ProductId = (int)id
            };

            //if (product == null)
            //{
            //    return NotFound();
            //}

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart model)
        {
            // Get the logged-in user's ID
            var GetUserId = (ClaimsIdentity)User.Identity;
            var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.ApplicationUserId = UserId;

            // Check if the product already exists in the cart for the current user
            var existingCartItem = _unitOfWork.ShoppingCart
                .GetAll()
                .FirstOrDefault(x => x.ApplicationUserId == UserId && x.ProductId == model.Product.Id);

            if (existingCartItem != null)
            {
                // Update the existing cart item
                existingCartItem.Count += model.Count;

                // Update the entity in the database
                _unitOfWork?.ShoppingCart.Update(existingCartItem);
            }
            else
            {
                // Create a new cart item
                var newCartItem = new ShoppingCart
                {
                    ProductId = model.Product.Id,
                    Count = model.Count,
                    ApplicationUserId = UserId
                };

                // Add the new entity to the database
                _unitOfWork?.ShoppingCart.Add(newCartItem);
            }

            // Save changes to the database
            _unitOfWork?.Save();

            // Redirect to the Index action
            return RedirectToAction(nameof(Index));
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
