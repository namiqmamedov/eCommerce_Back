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
    public class SizeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SizeController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Size> sizes = _context.Sizes.OrderBy(x => x.ID);

            return View(PagiNationList<Size>.Create(sizes, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Size size)
        {
            if (!ModelState.IsValid) return View();

            size.Name = size.Name.Trim();
            size.CreatedAt = DateTime.Now;

            await _context.Sizes.AddAsync(size);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Size size  = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == ID);

            if (size == null) return NotFound();

            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Size size)
        {
            if (ID == null) return BadRequest();

            Size sizes = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == ID);

            sizes.Name = size.Name.Trim();
            sizes.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Size> sizes = _context.Sizes.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (sizes == null) return NotFound();

            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == ID);

            if (size == null) return NotFound();

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();

            return PartialView("_SizeIndexPartial", PagiNationList<Size>.Create(sizes, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == ID);

            if (size == null) return NotFound();

            return View(size);
        }
    }
}
