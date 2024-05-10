using Wolmart.MVC.Models;

namespace Wolmart.MVC.ViewModels.About
{
    public class AboutVM
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<Award> Awards { get; set; }
    }
}
