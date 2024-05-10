namespace Wolmart.MVC.Models
{
    public class ProductSize : BaseEntity
    {
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int SizeID { get; set; }
        public Size Size { get; set; }
        public int Count { get; set; }
    }
}
