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
    public class AwardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AwardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Award> awards = _context.Awards.OrderBy(x => x.ID);

            return View(PagiNationList<Award>.Create(awards, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Award award)
        {
            if (!ModelState.IsValid) return View();

            if (award.File != null)
            {
                if (!award.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(award);
                }

                if (!award.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(award);
                }
                award.Logo = award.File.CreateImage(_env, "assets", "images");
            }
            else
            {
                ModelState.AddModelError("File", "Images is required");
                return View();
            }

            award.Text = award.Text.Trim();
            award.CreatedAt = DateTime.Now;

            await _context.Awards.AddAsync(award);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Award award = await _context.Awards.FirstOrDefaultAsync(x => x.ID == ID);

            if (award == null) return NotFound();

            return View(award);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Award award)
        {
            if (ID == null) return BadRequest();

            Award awards = await _context.Awards.FirstOrDefaultAsync(x => x.ID == ID);

            if (award.File != null)
            {
                if (!award.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(award);
                }

                if (!award.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size must be 20mb!");
                    return View(award);
                }
                awards.Logo = award.File.CreateImage(_env, "assets", "images");
            }

            awards.Text = award.Text.Trim();
            awards.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Award> awards = _context.Awards.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (awards == null) return NotFound();

            Award award  = await _context.Awards.FirstOrDefaultAsync(x => x.ID == ID);

            if (award == null) return NotFound();

            _context.Awards.Remove(award);
            await _context.SaveChangesAsync();

            return PartialView("_AwardIndexPartial", PagiNationList<Award>.Create(awards, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Award award = await _context.Awards.FirstOrDefaultAsync(x => x.ID == ID);

            if (award == null) return NotFound();

            return View(award);
        }
    }
}
