using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Country : BaseEntity
    {
        [StringLength(255)]
        [Required]
        public string Name { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
