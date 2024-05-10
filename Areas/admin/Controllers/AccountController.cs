using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolmart.MVC.Areas.admin.ViewModels.Account;
using Wolmart.MVC.Models;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("admin")]
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            AppUser appUser = await _userManager.FindByNameAsync(loginVM.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(appUser, loginVM.Password, true);


            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View(loginVM);
            }

            await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, loginVM.RemindMe, true);

            return RedirectToAction("Index", "Product", new { area = "admin" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }


        public async Task<IActionResult> Register()
        {
            AppUser appUser = new AppUser
            {
                FirstName = "admin",
                LastName = "admin",
                UserName = "admin",
                Email = "admin@mail.com"
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, "admin");

            if (!identityResult.Succeeded)
            {
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", "error");
                }
            }

            await _userManager.AddToRoleAsync(appUser, "Admin");

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> CreateRole()
        {
            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                if (await _roleManager.RoleExistsAsync(roleName))
                    continue;

                await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }

            return Ok();
        }
    }
}
