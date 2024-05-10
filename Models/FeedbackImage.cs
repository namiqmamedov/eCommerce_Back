using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class FeedbackImage : BaseEntity
    {
        public string Image { get; set; }
        public int FeedbackID { get; set; }
        public Feedback Feedback { get; set; }
    }
}
