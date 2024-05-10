using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Account;
using Wolmart.MVC.ViewModels.Profile;

namespace Wolmart.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ModelStateErrors"] = errorMessages;

                return RedirectToAction("Index", "Home");
            }

            AppUser appUser = new AppUser
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                UserName = registerVM.Username,
                Email = registerVM.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    TempData["ModelStateErrors"] = error.Description;
                }
                return RedirectToAction("Index", "Home");
            }

            await _userManager.AddToRoleAsync(appUser, "Member");

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == loginVM.LoginEmail.Trim().ToUpperInvariant());

            if (appUser == null)
            {
                TempData["ErrorMessage"] = "Email or password is incorrect";
                return RedirectToAction("Index", "Home");
            }

            if (!await _userManager.CheckPasswordAsync(appUser, loginVM.LoginPassword))
            {
                TempData["ErrorMessage"] = "Email or password is incorrect";
                return RedirectToAction("Index", "Home");
            }


            await _signInManager.SignInAsync(appUser, loginVM.RemindMe);

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Profile()
        {
            AppUser appUser = await _userManager.Users.Include(u => u.Orders).ThenInclude(u => u.OrderItems).ThenInclude(u => u.Product).FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (appUser == null) return RedirectToAction("index", "home");

            List<Order> orders = appUser.Orders != null ? appUser.Orders.ToList() : new List<Order>();

            ProfileVM profileVM = new ProfileVM
            {
                Name = appUser.FirstName,
                Surname = appUser.LastName,
                Email = appUser.Email,
                Orders = orders,
                Order = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Country).FirstOrDefaultAsync(x => x.AppUserId == appUser.Id)
            };

            return View(profileVM);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Profile(ProfileVM profileVM)
        {
            AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (appUser.Email != profileVM.Email && await _userManager.FindByEmailAsync(profileVM.Email) != null)
            {
                ModelState.AddModelError("", "The email address is already in use.");

                return View("Profile", profileVM);
            }

            appUser.FirstName = profileVM.Name;
            appUser.LastName = profileVM.Surname;
            appUser.Email = profileVM.Email;

            IdentityResult identityResult = await _userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                };

                return View("Profile", profileVM);
            }

            if (profileVM.CurrentPassword != null && profileVM.Password != null)
            {
                if (await _userManager.CheckPasswordAsync(appUser, profileVM.CurrentPassword) && profileVM.CurrentPassword == profileVM.Password)
                {
                    ModelState.AddModelError("", "The old password is the same as the password you typed now!");
                    return View("Profile", profileVM);
                }

                identityResult = await _userManager.ChangePasswordAsync(appUser, profileVM.CurrentPassword, profileVM.Password);

                if (!identityResult.Succeeded)
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    };

                    return View("Profile", profileVM);
                }
            }

            return RedirectToAction("Profile");
        }
    }
}
