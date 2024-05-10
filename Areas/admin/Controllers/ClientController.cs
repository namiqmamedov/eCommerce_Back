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
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ClientController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page)
        {
            IQueryable<Client> clients = _context.Clients.OrderBy(x => x.ID);

            return View(PagiNationList<Client>.Create(clients, page, 5));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (!ModelState.IsValid) return View();

            if (client.File != null)
            {
                if (!client.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(client);
                }

                if (!client.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("File", "The maximum size must be 20mb!");
                    return View(client);
                }
                client.Image = client.File.CreateImage(_env, "assets", "images", "brands");
            }
            else
            {
                ModelState.AddModelError("File", "Images is required");
                return View();
            }

            client.CreatedAt = DateTime.Now;

            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? ID)
        {
            if (ID == null) return BadRequest();

            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.ID == ID);

            if (client == null) return NotFound();

            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? ID, Client client)
        {
            if (ID == null) return BadRequest();

            Client clients = await _context.Clients.FirstOrDefaultAsync(x => x.ID == ID);

            if (client.File != null)
            {
                if (!client.File.CheckFileType())
                {
                    ModelState.AddModelError("File", "File type must be jpg or png.");
                    return View(client);
                }

                if (!client.File.CheckFileSize(20000))
                {
                    ModelState.AddModelError("MainFile", "The maximum size must be 20mb!");
                    return View(client);
                }
                clients.Image = client.File.CreateImage(_env, "assets", "images", "brands");
            }

            clients.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(int? ID, int page)
        {
            IQueryable<Client> clients = _context.Clients.OrderByDescending(x => x.ID == ID);

            if (ID == null) return BadRequest();

            if (clients == null) return NotFound();

            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.ID == ID);

            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return PartialView("_ClientIndexPartial", PagiNationList<Client>.Create(clients, page, 5));
        }

        [HttpGet]
        public async Task<IActionResult> Read(int? ID)
        {
            if (ID == null) return BadRequest();

            Client client = await _context.Clients.FirstOrDefaultAsync(x => x.ID == ID);

            if (client == null) return NotFound();

            return View(client);
        }
    }
}
