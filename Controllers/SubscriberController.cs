using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace Wolart.MVC.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly AppDbContext _context;

        public SubscriberController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(Subscriber subscriber)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Email", "Please enter correct email");
            }

            subscriber.Email = subscriber.Email.Trim();
            subscriber.CreatedTime = DateTime.Now;

            await _context.Subscribers.AddAsync(subscriber);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
