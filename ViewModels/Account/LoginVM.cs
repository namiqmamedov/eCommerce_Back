using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email cannot be null!")]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required(ErrorMessage = "Password cannot be null!")]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }

        public bool RemindMe { get; set; }
    }
}
