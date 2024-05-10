using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public UserController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(int page)
        {
            var users = await _userManager.Users.ToListAsync();

            var usersQueryable = users.AsQueryable(); 

            return View(PagiNationList<AppUser>.Create(usersQueryable, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(string ID)
        {
            if (ID == null) return BadRequest();

            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == ID.ToString());

            if (appUser == null) return NotFound();

            return View(appUser);
        }

        public IActionResult SearchUsers(string search, int page = 1)
        {
            var users = _userManager.Users.ToList();

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(search) || u.FirstName.ToLower().Contains(search) || u.LastName.ToLower().Contains(search) || u.Email.ToLower().Contains(search)).ToList();
            }
            else
            {
                users = users.ToList();
            }

            var paginatedUsers = PagiNationList<AppUser>.Create(users.AsQueryable(), page, 5);

            return PartialView("_UserIndexPartial", paginatedUsers);
        }
    }
}
