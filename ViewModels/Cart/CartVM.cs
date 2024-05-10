namespace Wolmart.MVC.ViewModels.Cart
{
    public class CartVM
    {
        public int ProductID { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public string Color { get; set; }
        public int ColorID { get; set; }
        public string Size { get; set; }
        public int SizeID { get; set; }
        public double Price { get; set; }
        public double DiscountedPrice { get; set; }
    }
}
