using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.ViewModels.Footer;

namespace Wolmart.MVC.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public FooterViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterVM footerVM = new FooterVM
            {
                Categories = await _context.Categories.ToListAsync(),
                Subcategories = await _context.Subcategories.ToListAsync(),
                Settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value),  
            };

            return View(await Task.FromResult(footerVM));
        }
    }
}
