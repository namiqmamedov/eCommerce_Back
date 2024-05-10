using Microsoft.AspNetCore.Mvc;
using Wolmart.MVC.ViewModels.Header;

namespace Wolmart.MVC.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync(HeaderVM headerVM)
        {
            return View(await Task.FromResult(headerVM));
        }
    }
}
