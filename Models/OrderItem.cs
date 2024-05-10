using System.ComponentModel.DataAnnotations.Schema;

namespace Wolmart.MVC.Models
{
    public class OrderItem
    {
        public int ID { get; set; }
        public Guid OrderID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public Size Size { get; set; }
        public Color Color { get; set; }
    }
}
