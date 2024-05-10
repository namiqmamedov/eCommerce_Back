using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Color : BaseEntity
    {

        [StringLength(255)]
        [Required]
        public string Name { get; set; }
    }
}
