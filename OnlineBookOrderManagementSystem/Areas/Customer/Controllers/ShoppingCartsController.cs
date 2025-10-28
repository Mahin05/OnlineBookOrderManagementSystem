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
            var ShoppingCart = _unitOfWork.ShoppingCart.GetAll().Where(x => x.ApplicationUserId==UserId).Include(x => x.Product);
            ViewBag.TotalCart = ShoppingCart.Count();
            double totalOrder = 0;
            foreach (var cart in ShoppingCart)
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

        public IActionResult Summary()
        {
            var GetUserId = (ClaimsIdentity)User.Identity;
            var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var UserDeatils = _unitOfWork.applicationUser.Get(x => x.Id == UserId);
            var UserDeatils = _unitOfWork.applicationUser.GetAll().Where(x => x.Id == UserId).FirstOrDefault();



            var ShoppingCarts = _unitOfWork.ShoppingCart.GetAll().Where(x => x.ApplicationUserId == UserId).Include(x => x.Product);
            ViewBag.TotalCart = ShoppingCarts.Count();
            double totalOrder = 0;
            foreach (var cart in ShoppingCarts)
            {
                //double price = GetPriceBasedOnQty(cart);
                if (cart.Count <= 50)
                {
                    cart.Product.Price = cart.Product.Price;
                }
                else if (cart.Count <= 100)
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


            var ShoppingCart = new ShoppingCartVM
            {
                Name = UserDeatils.Name,
                PhoneNumber = UserDeatils.PhoneNumber,
                StreetAddress = UserDeatils.StreetAddress,
                City = UserDeatils.City,
                State = UserDeatils.State,
                PostalCode = UserDeatils.PostalCode,
                TotalOrderPrice = totalOrder,
                ShoppingCartList = ShoppingCarts
            };


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


        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var GetUserId = (ClaimsIdentity)User.Identity;
            var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var UserDeatils = _unitOfWork.applicationUser.Get(x => x.Id == UserId);
            var UserDeatils = _unitOfWork.applicationUser.GetAll().Where(x => x.Id == UserId).FirstOrDefault();

            var OrderDate = System.DateTime.Now;

            var ShoppingCarts = _unitOfWork.ShoppingCart.GetAll().Where(x => x.ApplicationUserId == UserId).Include(x => x.Product);

            ViewBag.TotalCart = ShoppingCarts.Count();
            double totalOrder = 0;
            foreach (var cart in ShoppingCarts)
            {
                //double price = GetPriceBasedOnQty(cart);
                if (cart.Count <= 50)
                {
                    cart.Product.Price = cart.Product.Price;
                }
                else if (cart.Count <= 100)
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

            var ShoppingCart = new ShoppingCartVM
            {
                Name = UserDeatils.Name,
                PhoneNumber = UserDeatils.PhoneNumber,
                StreetAddress = UserDeatils.StreetAddress,
                City = UserDeatils.City,
                State = UserDeatils.State,
                PostalCode = UserDeatils.PostalCode,
                TotalOrderPrice = totalOrder,
                ShoppingCartList = ShoppingCarts

            };

            var OrderHeader = new OrderHeader
            {
                ApplicationUserId= UserId,
                OrderDate=OrderDate,
                ShippingDate=OrderDate,
                OrderTotal=totalOrder,
                PaymentStatus="",
                OrderStatus="",
                TrackingNumber="",
                Carrier="",
                PaymentDate=OrderDate,
                PaymentDueDate=OrderDate,
                SessionId="",
                PaymentIntentId=""
            };
            ShoppingCart.OrderHeader=OrderHeader;

            if(ShoppingCart.ShoppingCartList.FirstOrDefault().ApplicationUser.CompanyId == 0)
            {
                //it is a regular customer
                ShoppingCart.OrderHeader.PaymentStatus=SD.PaymentStatusPending;
                ShoppingCart.OrderHeader.OrderStatus=SD.StatusPending;
            }
            else
            {
                //it is a company user
                ShoppingCart.OrderHeader.PaymentStatus=SD.PaymentStatusDelayedPayment;
                ShoppingCart.OrderHeader.OrderStatus=SD.StatusApproved;
            }
            _unitOfWork.OrderHeader.Add(ShoppingCart.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCart.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCart.OrderHeader.Id,
                    Price = cart.Product.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            //if (ShoppingCart.ShoppingCartList.SingleOrDefault().ApplicationUser.CompanyId == 0)
            //{
            //    //customer regular account and payment track
            //    //stripe logic

            //}

            return RedirectToAction(nameof(OrderConfirmation), new {id=ShoppingCart.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}
