using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Message : BaseEntity
    {
        [Required]
        [StringLength(600)]
        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Fullname { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Text { get; set; }
    }
}
