using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Models.ViewModel;
using OnlineBookOrderManagementSystem.Repositories.IRepository;

namespace OnlineBookOrderManagementSystem.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartsController(ApplicationDBContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: Customer/ShoppingCarts
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var GetUserId = (ClaimsIdentity)User.Identity;
            var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;
            var ShoppingCart = _unitOfWork.ShoppingCart.GetAll().Where(x=>x.ApplicationUserId==UserId).Include(x=>x.Product);
            ViewBag.TotalCart = ShoppingCart.Count();
            double totalOrder = 0;
            foreach(var cart in ShoppingCart)
            {
                //double price = GetPriceBasedOnQty(cart);
                if (cart.Count <= 50)
                {
                    cart.Product.Price = cart.Product.Price;
                }
                else if (cart.Count <=100)
                {
                    cart.Product.Price = cart.Product.Price50;
                }
                else if (cart.Count > 100)
                {
                    cart.Product.Price = cart.Product.Price100;
                }
                totalOrder += cart.Product.Price * cart.Count;
            }
            ViewBag.TotalOrder = totalOrder;
            return View(ShoppingCart);
        }

        public IActionResult plus(int? cartId)
        {
            ShoppingCart ShoppingCart = _unitOfWork.ShoppingCart.Get(x => x.id == cartId).GetAwaiter().GetResult();
            ShoppingCart.Count += 1;
            _unitOfWork.ShoppingCart.Update(ShoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult minus(int? cartId)
        {
            ShoppingCart ShoppingCart = _unitOfWork.ShoppingCart.Get(x => x.id == cartId).GetAwaiter().GetResult();
            ShoppingCart.Count -= 1;
            _unitOfWork.ShoppingCart.Update(ShoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public double GetPriceBasedOnQty(ShoppingCart model)
        {
            if (model.Count <= 50)
            {
                return model.Product.Price;
            }
            else
            {
                if (model.Count <= 100)
                {
                    return model.Product.Price50;
                }
                else
                {
                    return model.Product.Price100;
                }
            }
        }

        // GET: Customer/ShoppingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts
                .Include(s => s.ApplicationUser)
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.id == id);
            if (shoppingCart == null)
            {
                return NotFound();
            }

            return View(shoppingCart);
        }

        // GET: Customer/ShoppingCarts/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.products, "Id", "Author");
            return View();
        }

        // POST: Customer/ShoppingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,ProductId,Count,ApplicationUserId")] ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shoppingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.products, "Id", "Author", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: Customer/ShoppingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCart = await _context.ShoppingCarts.FindAsync(id);
            if (shoppingCart == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.products, "Id", "Author", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // POST: Customer/ShoppingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,ProductId,Count,ApplicationUserId")] ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartExists(shoppingCart.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", shoppingCart.ApplicationUserId);
            ViewData["ProductId"] = new SelectList(_context.products, "Id", "Author", shoppingCart.ProductId);
            return View(shoppingCart);
        }

        // GET: Customer/ShoppingCarts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var shoppingCart = await _context.ShoppingCarts
        //        .Include(s => s.ApplicationUser)
        //        .Include(s => s.Product)
        //        .FirstOrDefaultAsync(m => m.id == id);
        //    if (shoppingCart == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(shoppingCart);
        //}

        // POST: Customer/ShoppingCarts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int cartId)
        public async Task<IActionResult> Delete(int cartId)
        {
            var shoppingCart = await _context.ShoppingCarts.FindAsync(cartId);
            if (shoppingCart != null)
            {
                _unitOfWork.ShoppingCart.Remove(shoppingCart);
            }

            _unitOfWork.Save();
            TempData["success"] = "Item Removed!";
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //ShoppingCartVM = new()
            //{
            //    ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            //    includeProperties: "Product"),
            //    OrderHeader = new()
            //};

            //ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            //ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            //ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            //ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            //ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            //ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            //ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            //foreach (var cart in ShoppingCartVM.ShoppingCartList)
            //{
            //    cart.Price = GetPriceBasedOnQuantity(cart);
            //    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            //}
            //return View(ShoppingCartVM);
            return View();
        }


        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCarts.Any(e => e.id == id);
        }
    }
}
