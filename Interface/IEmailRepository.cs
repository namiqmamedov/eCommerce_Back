using Wolmart.MVC.Models;

namespace Wolmart.MVC.Interface
{
    public interface IEmailRepository
    {
        Task GenerateLinkForNewProduct(Product product);
    }
}
