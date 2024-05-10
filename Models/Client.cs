using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class Client : BaseEntity
    {
        public string Image { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
