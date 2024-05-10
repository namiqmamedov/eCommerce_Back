using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Interface;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.Services
{
    public class LayoutServices : ILayoutServices
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LayoutServices(AppDbContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Subcategory>> GetSubcategories()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<List<CartVM>> GetCart()
        {
            string cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];

            List<CartVM> cartVMs = null;

            if(!string.IsNullOrWhiteSpace(cart))
            {
                cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cart);
            }
            else
            {
                cartVMs = new List<CartVM>();   
            }

            foreach (CartVM cartVM in cartVMs)
            {
                Product product = await _context.Products.Include(x=>x.ProductColors).FirstOrDefaultAsync(x => x.ID == cartVM.ProductID);
                Color color = await _context.Colors.FirstOrDefaultAsync(x => x.ID == cartVM.ColorID);
                Size size = await _context.Sizes.FirstOrDefaultAsync(x => x.ID == cartVM.SizeID);

                cartVM.Title = product.Name;
                cartVM.Image = product.MainImage;
                cartVM.Color = color.Name;
                cartVM.Size = size.Name;
            }

            return cartVMs;
        }

        public async Task<Dictionary<string, string>> GetSettingAsync()
        {
            return await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);
        }
    }
}
