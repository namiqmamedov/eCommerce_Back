using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.View;

namespace Wolmart.MVC.ViewModels.Home
{
    public class HomeVM
    {
        public List<Product> Products { get; set; }
        public List<Client> Clients { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Subcategory> Subcategories { get; set; }
        public List<ViewVM> ViewVMs { get; set; }
        public Dictionary<string, string> Settings { get; set; }

    }
}
