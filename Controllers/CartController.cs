using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);    
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return View(await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> GetCart()
        {
            string cart = Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> GetDetailCart()
        {
            string cart = Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return PartialView("_DetailCartPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> GetCartIndex()
        {
            string cart = Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if (!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();
            }

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> addToCart(int? ID, int? value, int? colorID, int? sizeID, int? price, int? discountedPrice)
        {
            if (ID == null) return BadRequest();

            if(colorID == null) colorID = 1;

            if (sizeID == null) sizeID = 1;

            Product product = await _context.Products
                .Include(x=>x.ProductColors)
                .FirstOrDefaultAsync(x => x.ID == ID);
            Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == colorID);
            Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == sizeID);

            if (product == null) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }

            else
            {
                cartVMs = new List<CartVM>();
            }

            if (value != null && cartVMs.Any(x => x.ProductID == ID && x.SizeID == sizeID && x.ColorID == colorID))
            {
                cartVMs.First(x => x.ProductID == ID && x.SizeID == sizeID && x.ColorID == colorID).Count += value.Value;
            }

            if (cartVMs.Exists(x => x.ProductID == ID && x.SizeID == sizeID && x.ColorID == colorID))
            {
                cartVMs.Find(x => x.ProductID == ID && x.SizeID == sizeID && x.ColorID == colorID).Count++;
            }


            else
            {
                CartVM cartVM = new CartVM
                {
                    ProductID = product.ID,
                    Price = price > 0 ? (int)price : product.ProductColors.Min(x=>x.Price),
                    DiscountedPrice = discountedPrice > 0 ? discountedPrice.Value : product.ProductColors.Min(x => x.DiscountedPrice ?? 0),
                    SizeID = size.ID,
                    ColorID = color.ID,
                    
                    Count = value ?? 1
                };

                cartVMs.Add(cartVM);
            }

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> RemoveCart(int? ID,int? colorID, int? sizeID)
        {
            if (ID == null) return BadRequest();

            if (!await _context.Products.AnyAsync(x => x.ID == ID)) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) return BadRequest();

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            CartVM cartVM = cartVMs.Find(x => x.ProductID == ID && x.ColorID == colorID && x.SizeID == sizeID);

            if (cartVM == null) return NotFound();

            cartVMs.Remove(cartVM);

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> UpdateCount(int? ID, int? colorID, int? sizeID, int count)
        {
            if (ID == null) return BadRequest();

            if (!await _context.Products.AnyAsync(x => x.ID == ID)) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

                CartVM cartVM = cartVMs.FirstOrDefault(x => x.ProductID == ID && x.ColorID == colorID && x.SizeID == sizeID);

                if(cartVM == null) return NotFound();

                cartVM.Count = count <= 0 ? 1 : count;

                cart = JsonConvert.SerializeObject(cartVMs);

                HttpContext.Response.Cookies.Append("cart", cart);
            }
            else
            {
                return BadRequest();
            }

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> RemoveFromCart(int? ID, int? colorID, int? sizeID)
        {
            if (ID == null) return BadRequest();

            if (!await _context.Products.AnyAsync(x => x.ID == ID)) return NotFound();

            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) return BadRequest();

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            CartVM cartVM = cartVMs.Find(x => x.ProductID == ID && x.ColorID == colorID && x.SizeID == sizeID);

            if (cartVM == null) return NotFound();

            cartVMs.Remove(cartVM);

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }
        public async Task<IActionResult> RemoveAllCart()
        {
            string cart = HttpContext.Request.Cookies["cart"];

            if (string.IsNullOrWhiteSpace(cart)) return BadRequest();

            List<CartVM> cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);

            cartVMs.RemoveAll(x => true);

            cart = JsonConvert.SerializeObject(cartVMs);

            HttpContext.Response.Cookies.Append("cart", cart);

            return PartialView("_CartIndexPartial", await _getCartItemAsync(cartVMs));
        }
        private async Task<List<CartVM>> _getCartItemAsync(List<CartVM> cartVMs) 
        {
            foreach (CartVM item in cartVMs)
            {
                Product product = await _context.Products.Include(x=>x.ProductColors).FirstOrDefaultAsync(x => x.ID == item.ProductID);
                Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == item.ColorID);
                Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == item.SizeID);

                item.Image = product.MainImage;
                item.Title = product.Name;
                item.Color = color.Name;
                item.Size = size.Name;
            }

            return cartVMs;
        }
    }
}
