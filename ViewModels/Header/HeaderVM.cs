using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Account;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.ViewModels.Header
{
    public class HeaderVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<Category> Categories { get; set; }
        public List<Subcategory> Subcategories { get; set; }
        public List<CartVM> CartVMs { get; set; }
        public RegisterVM registerVM { get; set; }
        public LoginVM loginVM { get; set; }
    }
}
