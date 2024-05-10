using Wolmart.MVC.Models;

namespace Wolmart.MVC.ViewModels.Footer
{
    public class FooterVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public Subscriber Subscriber { get; set; }
        public List<Category> Categories { get; set; }
        public List<Subcategory> Subcategories { get; set; }
    }
}
