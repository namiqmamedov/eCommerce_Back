using Wolmart.MVC.Models;

namespace Wolmart.MVC.ViewModels.Contact
{
    public class ContactVM
    {
        public Message Message { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
