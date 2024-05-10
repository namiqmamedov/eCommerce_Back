using System.ComponentModel.DataAnnotations;

namespace Wolmart.MVC.Models
{
    public class Subscriber
    {
        public int ID { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
