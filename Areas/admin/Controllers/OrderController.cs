using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Enums;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels;

namespace Wolmart.MVC.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int page)
        {
            IQueryable<Order> order = _context.Orders.Include(x => x.OrderItems).OrderBy(x => x.ID);

            return View(PagiNationList<Order>.Create(order, page, 5));
        }

        public async Task<IActionResult> Update(Guid? ID)
        {
            if (ID == null) return BadRequest();

            Order order = await _context.Orders.Include(x => x.OrderItems).ThenInclude(x => x.Product).Include(x => x.Country).FirstOrDefaultAsync(x => x.ID == ID);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(Guid? ID, OrderStatus orderStatus, string comment)
        {
            if (ID == null) return BadRequest();

            Order order = await _context.Orders.FirstOrDefaultAsync(x => x.ID == ID);

            if (order == null) return NotFound();

            order.OrderStatus = orderStatus;
            order.Comment = comment?.Trim();
            order.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
