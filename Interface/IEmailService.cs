using Wolmart.MVC.Models;

namespace Wolmart.MVC.Interface
{
    public interface IEmailService
    {
        Task SendEmailForNewProduct(UserEmailOptions userEmailOptions);
    }
}
