using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.HelperMethods;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;
using OnlineBookOrderManagementSystem.Repositories.Repository;

namespace OnlineBookOrderManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IProductReposiory productReposiory;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Products
        public IActionResult Index()
        {
            var GetUserId = (ClaimsIdentity)User.Identity;
            var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userName = _unitOfWork.applicationUser.GetAll().Where(x => x.Id == UserId).FirstOrDefault().Name;

            ViewBag.UserName = userName;

            var products = _unitOfWork.Product.GetAll().Include(x=>x.Category);
            return View(products);
        }


        [HttpGet("DownloadReport")]
        public async Task<IActionResult> DownloadReport([FromQuery] string UserName)
        {
            var products = _unitOfWork.Product.GetAll().Include(x => x.Category);

            string mimeType = "application/pdf";
            string reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "ProductRPT.rdlc");

            var datatable = Helpers.ListToDataTable(products.ToList());
            var localReport = new LocalReport(reportPath);
            localReport.AddDataSource("dsProducts", datatable);

            // Pass parameters from API request
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters?.Add("PrintedBy", UserName ?? "Unknown");

            var res = localReport.Execute(RenderType.Pdf, 1, parameters, mimeType);

            return File(res.MainStream, mimeType);
        }


        //public async Task<FileContentResult> DownloadReport()
        //{
        //    var products = _unitOfWork.Product.GetAll().Include(x => x.Category);
        //    var GetUserId = (ClaimsIdentity)User.Identity;
        //    var UserId = GetUserId.FindFirst(ClaimTypes.NameIdentifier).Value;
        //    var userName = _unitOfWork.applicationUser.GetAll().Where(x=>x.Id==UserId).FirstOrDefault().Name;


        //    //string format = "PDF";
        //    //string extension = "pdf";
        //    string mimeType = "application/pdf";


        //    //string embaddedPath = "OnlineBookOrderManagementSystem.wwwroot.Reports.ProductRPT.rdlc";
        //    string reportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\ProductRPT.rdlc";

        //    var datatable = Helpers.ListToDataTable(products.ToList());

        //    var localReport = new LocalReport(reportPath);
        //    localReport.AddDataSource("dsProducts", datatable);

        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters?.Add("PrintedBy", userName);

        //    // Open report in a new window
        //    var res = localReport.Execute(RenderType.Pdf, 1, parameters, mimeType);
        //    return File(res.MainStream, mimeType);

        //}

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var product = await _context.products
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var product = await _unitOfWork.Product.Get(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Upsert(int? Id)
        {
            // Initialize the Category List for the dropdown
            IEnumerable<SelectListItem> CategoryList = (_unitOfWork.Category.GetAll())
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            ViewBag.CategoryList = CategoryList;

            // Check if this is an Insert or Update operation
            if (Id == null)
            {
                return View(new Product());
            }
            else
            {
                // Update: Fetch the product for the given Id
                var product = await _unitOfWork.Product.Get(m => m.Id == Id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }


        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productFolder = Path.Combine(wwwRootPath, @"Images\product");
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productFolder, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ImageUrl = @"\Images\product\" + fileName;
                    //productModel.Product.ImageUrl = productFolder+fileName;
                }
                if (product.Id == null)
                {
                    TempData["success"] = "Product Created Successfully!";
                    _unitOfWork.Product.Add(product);
                }
                else
                {
                    TempData["success"] = "Product Updated Successfully!";
                    _unitOfWork.Product.Update(product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.Product.Get(m=>m.Id==id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Discription,ISBN,Author,ListPrice,Price,Price50,Price100")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Product.Update(product);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _unitOfWork.Product.Get(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _unitOfWork.Product.Get(m=>m.Id==id);
            if (product != null)
            {
                TempData["success"] = "Product Deleted Successfully!";
                await _unitOfWork.Product.Remove(product);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.products.Any(e => e.Id == id);
        }


        #region API
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll().Include(x=>x.Category);
            return Json(new { data = products });
        }
        #endregion

    }
}
