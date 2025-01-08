using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineBookOrderManagementSystem.Data;
using OnlineBookOrderManagementSystem.Models;
using OnlineBookOrderManagementSystem.Repositories.IRepository;
using OnlineBookOrderManagementSystem.Repositories.Repository;

namespace OnlineBookOrderManagementSystem.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly ICategoryReposiory categoryReposiory;

        public CategoryController(ApplicationDBContext context,ICategoryReposiory categoryReposiory)
        {
            _context = context;
            this.categoryReposiory = categoryReposiory;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = categoryReposiory.GetAll();
            //var categoriess = _context.categories.ToListAsync();
            //return View(await _context.categories.ToListAsync());
            return View(await categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.categories
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var category = await categoryReposiory.Get(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(category);
                await categoryReposiory.Add(category);
                await categoryReposiory.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.categories.FindAsync(id);
            var category = await categoryReposiory.Get(x=>x.Id==id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DisplayOrder")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(category);
                    await categoryReposiory.Update(category);
                    //await _context.SaveChangesAsync();
                    await categoryReposiory.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var category = await _context.categories.FirstOrDefaultAsync(m => m.Id == id);
            var category = await categoryReposiory.Get(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var category = await _context.categories.FindAsync(id);
            var category = await categoryReposiory.Get(x=>x.Id==id);
            if (category != null)
            {
                //_context.categories.Remove(category);
                await categoryReposiory.Remove(category);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.Id == id);
        }
    }
}
