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
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Slider> sliders = _context.Sliders.OrderBy(x => x.ID);

            return View(PagiNationList<Slider>.Create(sliders, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View();

            if (slider.File != null)
            {
                if (!slider.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(slider);
                }


                if (!slider.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(slider);
                }

                slider.Image = slider.File.CreateImage(_env, "assets", "images", "slides");
            }
            else
            {
                ModelState.AddModelError("File", "Images is required");
                return View();
            }

            if (slider.SliderFile != null)
            {
                if (!slider.SliderFile.CheckFileType())
                {
                    ModelState.AddModelError("SliderFile", "File type must be jpg or png.");
                    return View(slider);
                }

                if (!slider.SliderFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("SliderFile", "The maximum size must be 20mb!");
                    return View(slider);
                }

                slider.BackgroundImage = slider.SliderFile.CreateImage(_env, "assets", "images", "slides");
            }
            else
            {
                ModelState.AddModelError("SliderFile", "Images is required");
                return View();
            }

            slider.TopTitle = slider.TopTitle.Trim();
            slider.Title = slider.Title.Trim();
            slider.Description = slider.Description.Trim();
            slider.ShopLink = slider.ShopLink.Trim();
            slider.IsFirstImage = slider.IsFirstImage;
            slider.CreatedAt = DateTime.Now;

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == ID);

            if (slider == null) return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Slider slider)
        {
            if (ID == null) return BadRequest();

            Slider sliders = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == ID);

            if (slider.File != null)
            {
                if (!slider.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(slider);
                }

                if (!slider.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(slider);
                }
                sliders.Image = slider.File.CreateImage(_env, "assets", "images", "slides");
            }

            if (slider.SliderFile != null)
            {
                if (!slider.SliderFile.CheckFileType())
                {
                    ModelState.AddModelError("SliderFile", "File type must be jpg or png.");
                    return View(slider);
                }

                if (!slider.SliderFile.CheckFileSize(20000))
                {
                    ModelState.AddModelError("SliderFile", "The maximum size must be 20mb!");
                    return View(slider);
                }
                sliders.BackgroundImage = slider.SliderFile.CreateImage(_env, "assets", "images", "slides");
            }

            sliders.TopTitle = slider.TopTitle.Trim();
            sliders.Title = slider.Title.Trim();
            sliders.Description = slider.Description.Trim();
            sliders.ShopLink = slider.ShopLink.Trim();
            sliders.IsFirstImage = slider.IsFirstImage;
            sliders.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Slider> sliders = _context.Sliders.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (sliders == null) return NotFound();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == ID);

            if (slider == null) return NotFound();

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return PartialView("_SliderIndexPartial", PagiNationList<Slider>.Create(sliders, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Slider slider = await _context.Sliders.FirstOrDefaultAsync(x => x.ID == ID);

            if (slider == null) return NotFound();

            return View(slider);
        }
    }
}
