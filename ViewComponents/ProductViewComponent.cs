using Microsoft.AspNetCore.Mvc;
using Wolmart.MVC.Models;

namespace Wolmart.MVC.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Product product)
        {
            return View(await Task.FromResult(product));    
        }
    }
}
