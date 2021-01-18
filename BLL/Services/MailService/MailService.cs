using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NewsletterAPI.Entities;
using NewsletterAPI.Models;
using System;
using System.Threading.Tasks;

namespace NewsletterAPI.BLL.Services.MailingService
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.From, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendVerificationCodeAsync(NewsletterUser user)
        {
            EmailMessage emailMessage = new()
            {
                ToEmail = user.Email,
                Subject = "Verifiera din e-postadress"
            };

            string verifyLink = String.Format("<a href=\"https://randomwebbsida.se/verifiera-epost-for-nyhetsbrev?verifieringskod={0}&epost={1}\">Verifiera</a>", user.VerificationCode, user.Email);
            emailMessage.Body = $"<h3>Hej</h3>{Environment.NewLine}" +
                $"<p>Du håller på att registrera för nyhetsbrev med den här e-postadressen. Verifiera genom att klicka på länken nedan inom 15 minuter:</p>{Environment.NewLine}" +
                $"<p>{verifyLink}</p>" +
                $"<p>Har du inte gjort det kan du ignorera det här mejlet</p>";

            await CreateEmail(emailMessage);
        }

        public async Task SendNewsletterConfirmation(NewsletterUser user)
        {
            EmailMessage emailMessage = new()
            {
                ToEmail = user.Email,
                Subject = "Din e-postadress är registrerad"
            };

            emailMessage.Body = $"<h3>Hej</h3><p>Tack! Din e-postadress är nu registrerad för nyhetsbrev</p>";
            await CreateEmail(emailMessage);
        }

        private async Task CreateEmail(EmailMessage emailMessage)
        {
            MimeMessage email = new()
            {
                Sender = MailboxAddress.Parse(_mailSettings.From),
                Subject = emailMessage.Subject
            };

            email.To.Add(MailboxAddress.Parse(emailMessage.ToEmail));
           
            BodyBuilder bodyBuilder = new() { HtmlBody = emailMessage.Body };
            email.Body = bodyBuilder.ToMessageBody();
            await SendEmailAsync(email);
        }
    }
}
