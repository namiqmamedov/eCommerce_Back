using System.ComponentModel.DataAnnotations;
using Wolmart.MVC.Enums;

namespace Wolmart.MVC.Models
{
    public class Order 
    {
        public Guid ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? UpdatedAt { get; set; }
        public double TotalPrice { get; set; }

        [Required]
        [StringLength(255)]
        [MinLength(3)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required]
        [MinLength(3)]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Surname { get; set; }
        [Required]
        [StringLength(255)]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        [Phone(ErrorMessage = "Please enter a valid phone field.")]
        public string Phone { get; set; }
        [Required]
        [StringLength(255)]
        public string AddressFirst { get; set; }
        [StringLength(255)]
        public string AddressSecond { get; set; }
        [Required]
        [StringLength(255)]
        public string TownCity { get; set; }
        [Required]
        [StringLength(255)]
        [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Postal code must be exactly 5 digits.")]
        public string ZipCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        [StringLength(525)]
        [DataType(DataType.Text)]
        public string Comment { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CountryID { get; set; }
        public Country Country { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
