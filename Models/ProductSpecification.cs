using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class ProductSpecification : BaseEntity
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
        public string Model { get; set;}
        [Required]
        public string Guarantee { get; set;}  
    }
}
