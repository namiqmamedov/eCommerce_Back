using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class Award : BaseEntity
    {
        [Required]
        [StringLength(255)]
        public string Text { get; set; }
        public string Logo { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
