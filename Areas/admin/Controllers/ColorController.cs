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
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ColorController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Color> colors = _context.Colors.OrderBy(x => x.ID);

            return View(PagiNationList<Color>.Create(colors, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Color color)
        {
            if (!ModelState.IsValid) return View();

            color.Name = color.Name.Trim();
            color.CreatedAt = DateTime.Now;

            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Color color  = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Color color)
        {
            if (ID == null) return BadRequest();

            Color colors = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            colors.Name = color.Name.Trim();
            colors.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Color> colors = _context.Colors.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (colors == null) return NotFound();

            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();

            return PartialView("_ColorIndexPartial", PagiNationList<Color>.Create(colors, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == ID);

            if (color == null) return NotFound();

            return View(color);
        }
    }
}
