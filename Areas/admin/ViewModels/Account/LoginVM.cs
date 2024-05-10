using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Areas.admin.ViewModels.Account
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Username cannot be null!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password cannot be null!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RemindMe { get; set; }
    }
}
