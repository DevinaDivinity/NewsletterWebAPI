using MimeKit;
using NewsletterAPI.Entities;
using System.Threading.Tasks;

namespace NewsletterAPI.BLL.Services.MailingService
{
    public interface IMailService
    {
        public Task SendEmailAsync(MimeMessage email);
        public Task SendVerificationCodeAsync(NewsletterUser user);
        public Task SendNewsletterConfirmation(NewsletterUser user);
    }
}
