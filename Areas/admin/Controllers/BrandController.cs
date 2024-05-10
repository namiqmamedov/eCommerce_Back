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
    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Brand> brands = _context.Brands.OrderBy(x => x.ID);

            return View(PagiNationList<Brand>.Create(brands, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid) return View();

            brand.Name = brand.Name.Trim();
            brand.CreatedAt = DateTime.Now;

            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Brand Brand)
        {
            if (ID == null) return BadRequest();

            Brand brands = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            brands.Name = Brand.Name.Trim();
            brands.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Brand> brands = _context.Brands.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (brands == null) return NotFound();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();

            return PartialView("_BrandIndexPartial", PagiNationList<Brand>.Create(brands, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Brand brand = await _context.Brands.FirstOrDefaultAsync(x => x.ID == ID);

            if (brand == null) return NotFound();

            return View(brand);
        }
    }
}
