using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class ProductColor : BaseEntity
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int ColorID { get; set; }
        public Color Color { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public double Price { get; set; }
        [Range(0, int.MaxValue)]
        public double? DiscountedPrice { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
    }
}
