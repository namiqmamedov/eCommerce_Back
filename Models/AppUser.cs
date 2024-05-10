using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.Models
{
    public class AppUser : IdentityUser
    {
        [StringLength(255)]
        public string FirstName { get; set; }
        [StringLength(255)]
        public string LastName { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
