using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Brand : BaseEntity
    {

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
