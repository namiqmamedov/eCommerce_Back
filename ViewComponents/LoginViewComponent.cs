using Microsoft.AspNetCore.Mvc;
using Wolmart.MVC.Areas.admin.ViewModels.Account;
using Wolmart.MVC.ViewModels.Account;
using Wolmart.MVC.ViewModels.Footer;

namespace Wolmart.MVC.ViewComponents
{
    public class LoginViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(RegisterVM registerVM)
        {
            return View(await Task.FromResult(registerVM));
        }
    }
}
