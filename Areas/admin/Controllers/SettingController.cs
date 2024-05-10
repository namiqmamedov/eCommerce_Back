using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Extension;
using Wolmart.MVC.Helpers;
using Wolmart.MVC.Models;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

            return View(settings);
        }

        [HttpGet]
        public async Task<IActionResult> Update(string key)
        {
            if (key == null) return BadRequest();

            Setting setting = await _context.Settings.FirstOrDefaultAsync(k => k.Key == key);

            if (setting == null) return NotFound();

            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string key, Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }

            if (key == null) return BadRequest();

            if (key != setting.Key) return NotFound();

            Setting dbSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Key == key);

            if (dbSetting == null) return NotFound();

            if (setting.File != null)
            {
                if (!setting.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(setting);
                }

                if (!setting.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(setting);
                }

                FileHelper.DeleteFile(_env, dbSetting.Value, "assets", "images");

                dbSetting.Value = setting.File.CreateImage(_env, "assets", "images");
            }

            if (setting.File == null)
            {
                dbSetting.Value = setting.Value.Trim();
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Read(string key)
        {
            if (key == null) return BadRequest();

            Setting setting = await _context.Settings.FirstOrDefaultAsync(k => k.Key == key);

            if (setting == null) return NotFound();

            return View(setting);
        }
    }
}
