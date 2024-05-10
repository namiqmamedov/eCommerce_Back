using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;
using Wolmart.MVC.ViewModels.View;

namespace Wolmart.MVC.Controllers
{
    public class ViewController : Controller
    {
        private readonly AppDbContext _context;

        public ViewController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddView(int? ID)
        {
            if (ID != null) return BadRequest();

            Product product = await _context.Products.FirstOrDefaultAsync(x => x.ID == ID);

            if (product != null) return NotFound();

            string view = HttpContext.Request.Cookies["view"];

            List<ViewVM> viewVMs = null;

            if (!string.IsNullOrWhiteSpace(view)) 
            {
                viewVMs = JsonConvert.DeserializeObject<List<ViewVM>>(view);    
            }
            else
            {
                viewVMs = new List<ViewVM>();
            }

            ViewVM viewVM = new ViewVM
            {
                ProductID = product.ID,
            };

            viewVMs.Add(viewVM);

            view = JsonConvert.SerializeObject(viewVMs);

            HttpContext.Response.Cookies.Append("view", view);

            return View(viewVM);
        }
    }
}
