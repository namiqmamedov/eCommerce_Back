using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class Slider : BaseEntity
    {
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string TopTitle { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Title { get; set; }

        [StringLength(255)]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public bool IsFirstImage { get; set; }
        [Required]
        [StringLength(255)]
        [DataType(DataType.Text)]
        public string ShopLink { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        
        [NotMapped]
        public IFormFile SliderFile { get; set; }
    }
}
