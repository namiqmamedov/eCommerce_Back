using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CountryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CountryController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Country> countries = _context.Countries.OrderBy(x => x.ID);

            return View(PagiNationList<Country>.Create(countries, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (!ModelState.IsValid) return View();

            country.Name = country.Name.Trim();

            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.ID == ID);

            if (country == null) return NotFound();

            return View(country);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Country country)
        {
            if (ID == null) return BadRequest();

            Country countries = await _context.Countries.FirstOrDefaultAsync(x => x.ID == ID);

            countries.Name = country.Name.Trim();

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Country> countries = _context.Countries.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (countries == null) return NotFound();

            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.ID == ID);

            if (country == null) return NotFound();

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return PartialView("_CountryIndexPartial", PagiNationList<Country>.Create(countries, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Country country = await _context.Countries.FirstOrDefaultAsync(x => x.ID == ID);

            if (country == null) return NotFound();

            return View(country);
        }
    }
}
