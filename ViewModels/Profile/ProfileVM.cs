using System.ComponentModel.DataAnnotations;
using Wolmart.MVC.Models;

namespace Wolmart.MVC.ViewModels.Profile
{
    public class ProfileVM
    {
        [StringLength(255)]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [StringLength(255)]
        [MinLength(3, ErrorMessage = "Surname must be at least 3 characters long.")]
        [DataType(DataType.Text)]
        public string Surname { get; set; }
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }

        public List<Order> Orders { get; set; }
        public Order Order { get; set; }
    }
}
