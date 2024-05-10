using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.ViewModels.Orders
{
    public class OrdersVM
    {
        public Order Order { get; set; }
        public List<CartVM> CartVMs { get; set; }
    }
}
