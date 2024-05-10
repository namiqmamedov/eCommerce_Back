using Microsoft.EntityFrameworkCore;
using Wolmart.MVC.DAL;
using Wolmart.MVC.Interface;
using Wolmart.MVC.Models;

namespace Wolmart.MVC.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public EmailRepository(IEmailService emailService, AppDbContext context)
        {
            _emailService = emailService;
            _context = context;
        }
        public async Task SendNewProductNotification(Product product)
        {
            List<Subscriber> subscribers = await _context.Subscribers.ToListAsync();

            foreach (var subscriber in subscribers)
            {
                UserEmailOptions options = new UserEmailOptions
                {
                    ToEmails = new List<string>() { subscriber.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("{{Email}}", subscriber.Email),
                        new KeyValuePair<string, string>("{{Product}}", product.Name),
                        new KeyValuePair<string, string>("{{ProductID}}", product.ID.ToString())
                    }
                };

                await _emailService.SendEmailForNewProduct(options);
            }
        }

        public async Task GenerateLinkForNewProduct(Product product)
        {
            await SendNewProductNotification(product);
        }
    }
}
