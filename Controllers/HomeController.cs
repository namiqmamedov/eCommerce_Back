using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Account;
using Wolmart.MVC.ViewModels.Cart;
using Wolmart.MVC.ViewModels.Home;
using Wolmart.MVC.ViewModels.Shops;
using Wolmart.MVC.ViewModels.View;

namespace Wolmart.MVC.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public HomeController(AppDbContext context,RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<IActionResult> Index()
    {
        if (TempData["ErrorMessage"] != null)
        {
            ModelState.AddModelError("", TempData["ErrorMessage"].ToString());
        }

        if (TempData["ModelStateErrors"] != null)
        {
            ModelState.AddModelError("", TempData["ModelStateErrors"].ToString());
        }

        string view = Request.Cookies["view"];

        List<ViewVM> viewVMs = null;

        if (!string.IsNullOrWhiteSpace(view))
        {
            viewVMs = JsonConvert.DeserializeObject<List<ViewVM>>(view);
        }
        else
        {
            viewVMs = new List<ViewVM>();
        }

        HomeVM homeVM = new HomeVM
        {
            Clients = await _context.Clients.ToListAsync(),
            Products = await _context.Products.Include(x=>x.ProductColors).Include(x=>x.Feedbacks).ToListAsync(),
            Sliders = await _context.Sliders.ToListAsync(),
            Subcategories = await _context.Subcategories.ToListAsync(),
            Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),
            ViewVMs = viewVMs
        };

        return View(homeVM);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
