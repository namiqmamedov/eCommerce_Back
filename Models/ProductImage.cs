using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class ProductImage : BaseEntity
    {
        public string Image { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }

    }
}
