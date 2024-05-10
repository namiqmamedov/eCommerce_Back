using Wolmart.MVC.Models;
using Wolmart.MVC.ViewModels.Cart;

namespace Wolmart.MVC.ViewModels.Shops
{
    public class ShopVM
    {
        public Product Product { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductDescription> Description { get; set; }
        public List<ProductSpecification> Specifications { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<Feedback> Feedbacks { get; set; }
        public Feedback Feedback { get; set; }
        public List<FeedbackImage> FeedbackImages { get; set; }
        public List<Size> Sizes { get; set; }
    }
}
