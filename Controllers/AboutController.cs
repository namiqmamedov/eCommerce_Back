using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Wolmart.MVC.DAL;
using Wolmart.MVC.ViewModels.About;

namespace Wolmart.MVC.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _context;

        public AboutController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            AboutVM aboutVM = new AboutVM
            {
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key,x => x.Value),
                Awards = await _context.Awards.ToListAsync()
            };

            return View(aboutVM);
        }
    }
}
