using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SubcategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubcategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Subcategory> subcategories = _context.Subcategories.OrderBy(x => x.ID);

            return View(PagiNationList<Subcategory>.Create(subcategories, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Subcategories = await _context.Subcategories.Where(x=>x.ParentSubcategoryID == null).ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcategory subcategory)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Subcategories = await _context.Subcategories.Where(x => x.ParentSubcategoryID == null).ToListAsync();

            if (!ModelState.IsValid) return View();

            if(!await _context.Subcategories.AnyAsync(x=>x.ParentCategoryID == subcategory.ParentCategoryID))
            {
                ModelState.AddModelError("ParentCategoryID", "Please select a subcategory that matches the category");

                return View();
            }

            subcategory.Name = subcategory.Name.Trim();
            subcategory.CreatedAt = DateTime.Now;

            await _context.Subcategories.AddAsync(subcategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Subcategories = await _context.Subcategories.Where(x => x.ParentSubcategoryID == null).ToListAsync();

            if (ID == null) return BadRequest();

            Subcategory subcategory = await _context.Subcategories.FirstOrDefaultAsync(x => x.ID == ID);

            if (subcategory == null) return NotFound();

            return View(subcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Subcategory subcategory)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Subcategories = await _context.Subcategories.Where(x => x.ParentSubcategoryID == null).ToListAsync();

            if (ID == null) return BadRequest();

            if (!await _context.Subcategories.AnyAsync(x => x.ParentCategoryID == subcategory.ParentCategoryID))
            {
                ModelState.AddModelError("ParentCategoryID", "Please select a subcategory that matches the category");

                return View();
            }

            Subcategory subcategories = await _context.Subcategories.FirstOrDefaultAsync(x => x.ID == ID);

            subcategories.Name = subcategory.Name.Trim();
            subcategories.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Subcategory> subcategories = _context.Subcategories.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (subcategories == null) return NotFound();

            Subcategory subcategory = await _context.Subcategories.FirstOrDefaultAsync(x => x.ID == ID);

            if (subcategory == null) return NotFound();

            _context.Subcategories.Remove(subcategory);
            await _context.SaveChangesAsync();

            return PartialView("_SubcategoryIndexPartial", PagiNationList<Subcategory>.Create(subcategories, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            ViewBag.Categories = await _context.Categories.ToListAsync();
            ViewBag.Subcategories = await _context.Subcategories.Where(x => x.ParentSubcategoryID == null).ToListAsync();

            if (ID == null) return BadRequest();

            Subcategory subcategory = await _context.Subcategories.FirstOrDefaultAsync(x => x.ID == ID);

            if (subcategory == null) return NotFound();

            return View(subcategory);
        }
    }
}
