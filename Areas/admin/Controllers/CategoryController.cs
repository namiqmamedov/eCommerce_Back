using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.Extension;
using Microsoft.AspNetCore.Authorization;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CategoryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Category> categories = _context.Categories.OrderBy(x => x.ID);

            return View(PagiNationList<Category>.Create(categories, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();

            if (category.File != null)
            {
                if (!category.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(category);
                }


                if (!category.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(category);
                }
                category.Image = category.File.CreateImage(_env, "assets", "images", "categories");
            }
            else
            {
                ModelState.AddModelError("File", "Images is required");
                return View();
            }

            category.Name = category.Name.Trim();
            category.CreatedAt = DateTime.Now;

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Category category)
        {
            if (ID == null) return BadRequest();

            Category categories = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category.File != null)
            {
                if (!category.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(category);
                }

                if (!category.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size must be 20mb!");
                    return View(category);
                }
                categories.Image = category.File.CreateImage(_env, "assets", "images", "categories");
            }

            categories.Name = category.Name.Trim();
            categories.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Category> categories = _context.Categories.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (categories == null) return NotFound();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return PartialView("_CategoryIndexPartial", PagiNationList<Category>.Create(categories, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.ID == ID);

            if (category == null) return NotFound();

            return View(category);
        }
    }
}
