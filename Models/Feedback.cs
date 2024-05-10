using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class Feedback
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? CreatedAt { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }

        [Required]
        public int Rating { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public List<FeedbackImage> FeedbackImages { get; set; }

        [NotMapped]
        [MaxLength(5)]
        public IEnumerable<IFormFile> Files { get; set; }
    }
}
