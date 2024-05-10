using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class ProductDescription : BaseEntity
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
