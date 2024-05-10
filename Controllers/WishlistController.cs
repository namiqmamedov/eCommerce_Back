using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;
using Wolmart.MVC.ViewModels.Wishlist;

namespace Wolmart.MVC.Controllers
{
    public class WishlistController : Controller
    {
        private readonly AppDbContext _context;

        public WishlistController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string wishlist = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = null;

            if (!string.IsNullOrWhiteSpace(wishlist))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);
            }
            else
            {
                wishlistVMs = new List<WishlistVM>();
            }

            return View(await _getWishlistItemAsync(wishlistVMs));
        }

        public async Task<IActionResult> addToWishlist(int? ID)
        {
            if (ID == null) return BadRequest();

            Product product = await _context.Products.Include(x => x.ProductColors).FirstOrDefaultAsync(x => x.ID == ID);

            if (product == null) return NotFound();

            string wishlist = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = null;

            if (!string.IsNullOrWhiteSpace(wishlist))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);
            }

            else
            {
                wishlistVMs = new List<WishlistVM>();
            }

            if (wishlistVMs.Exists(x => x.ProductID == ID))
            {
                wishlistVMs.Find(x => x.ProductID == ID).Count++;
            }

            else
            {
                WishlistVM wishlistVM = new WishlistVM
                {
                    ProductID = product.ID,
                    TotalCount = product.Count,
                    Count =  1
                };

                wishlistVMs.Add(wishlistVM);
            }

            wishlist = JsonConvert.SerializeObject(wishlistVMs);

            HttpContext.Response.Cookies.Append("wishlist", wishlist);

            return Ok();
        }

        public async Task<IActionResult> RemoveCart(int? ID)
        {
            if (ID == null) return BadRequest();

            if (!await _context.Products.AnyAsync(x => x.ID == ID)) return NotFound();

            string wishlist = HttpContext.Request.Cookies["wishlist"];

            List<WishlistVM> wishlistVMs = new List<WishlistVM>();

            if (!string.IsNullOrWhiteSpace(wishlist))
            {
                wishlistVMs = JsonConvert.DeserializeObject<List<WishlistVM>>(wishlist);

                WishlistVM wishlistVM = wishlistVMs.Find(x => x.ProductID == ID);

                if (wishlistVM != null)
                {
                    wishlistVMs.Remove(wishlistVM);
                }

                wishlist = JsonConvert.SerializeObject(wishlistVMs);

                HttpContext.Response.Cookies.Append("wishlist", wishlist);
            }

            return PartialView("_WishlistIndexPartial", await _getWishlistItemAsync(wishlistVMs));
        }

        private async Task<List<WishlistVM>> _getWishlistItemAsync(List<WishlistVM> wishlistVMs)
        {
            foreach (WishlistVM item in wishlistVMs)
            {
                Product product = await _context.Products.Include(x => x.ProductColors).FirstOrDefaultAsync(x => x.ID == item.ProductID);

                item.Image = product.MainImage;
                item.Title = product.Name;
                item.TotalCount = product.Count;
            }

            return wishlistVMs;
        }
    }
}
