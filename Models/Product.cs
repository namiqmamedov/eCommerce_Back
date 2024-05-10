using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class Product : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Count { get; set; }
        public string MainImage { get; set; }
        public string HoverImage { get; set; }

        [Required]
        public int BrandID { get; set; }
        public Brand Brand { get; set; }

        [Required]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public List<ProductColor> ProductColors { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductDescription> ProductDescriptions { get; set; }
        public List<ProductSpecification> ProductSpecifications { get; set; }

        [NotMapped]
        [Required]
        public List<string> ProductDescription { get; set; } = new List<string>();

        [NotMapped]
        [Required]
        public IFormFile MainFile { get; set; }
        [NotMapped]
        [Required]
        public IFormFile HoverFile { get; set; }
        [NotMapped]
        [Required]
        [MaxLength(5)]
        public IEnumerable<IFormFile> Files { get; set; }

        [NotMapped]
        [Required]
        public List<double?> Prices { get; set; }

        [NotMapped]
        public List<double?> DiscountedPrices { get; set; }

        [NotMapped]
        public List<int> SizeIDs { get; set; }

        [NotMapped]
        public List<int?> SizeCounts { get; set; }

        [NotMapped]
        public List<int> ColorIDs { get; set; }

        [NotMapped]
        public List<int?> Counts { get; set; }
    }
}
