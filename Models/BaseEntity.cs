using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class BaseEntity
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}")]
        public DateTime? UpdatedAt { get; set; }
    }
}
