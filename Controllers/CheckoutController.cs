using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Enums;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;
using Wolmart.MVC.ViewModels.Orders;

namespace Wolmart.MVC.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public CheckoutController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<CartVM>> GetCart()
        {
            string cart = Request.Cookies["cart"];
            List<CartVM> cartItems = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartItems = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartItems = new List<CartVM>();
            }

            return cartItems;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Country = await _context.Countries.ToListAsync();

            AppUser appUser = null;

            if (User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            List<CartVM> cart = await GetCart();

            foreach (CartVM item in cart)
            {
                Product product = await _context.Products.Include(x => x.ProductColors).FirstOrDefaultAsync(x => x.ID == item.ProductID);
                Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == item.ColorID);
                Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == item.SizeID);

                item.Image = product.MainImage;
                item.Title = product.Name;
                item.Color = color.Name;
                item.Size = size.Name;
            }

            Order order = new Order();

            if (appUser != null)
            {
                order.Name = appUser.FirstName.Trim();
                order.Surname = appUser.LastName.Trim();
                order.Email = appUser.Email.Trim();
            }

            OrdersVM ordersVM = new OrdersVM
            {
                Order = order,
                CartVMs = cart
            };

            return View(ordersVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(Order order)
        {
            AppUser appUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

            List<OrderItem> orderItems = new List<OrderItem>();

            List<CartVM> cart = await GetCart();

            foreach (CartVM item in cart)
            {
                OrderItem orderItem = new OrderItem
                {
                    Price = item.DiscountedPrice > 0 ? item.DiscountedPrice : item.Price,
                    Count = item.Count,
                    ProductID = item.ProductID,
                    SizeID = item.SizeID,
                    ColorID = item.ColorID,
                    TotalPrice = item.DiscountedPrice > 0 ? item.DiscountedPrice * item.Count : item.Price * item.Count
                };

                orderItems.Add(orderItem);
            }

            order.OrderItems = orderItems;
            order.CreatedAt = DateTime.Now;
            order.AppUserId = appUser?.Id;
            order.OrderStatus = OrderStatus.Pending;
            order.TotalPrice = orderItems.Sum(x => x.TotalPrice);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return RedirectToAction("index", "home");
        }
    }
}
