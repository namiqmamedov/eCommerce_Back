﻿using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.ViewModels.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Firstname field cannot be empty!")]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lastname field cannot be empty!")]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Username field cannot be empty!")]
        [StringLength(255)]
        public string Username { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [Compare(nameof(Password), ErrorMessage = "Passwords must be the same!")]
        public string ConfirmPassword { get; set; }
    }
}
