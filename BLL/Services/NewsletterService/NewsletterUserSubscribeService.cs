using Microsoft.Extensions.Logging;
using NewsletterAPI.BLL.Services.MailingService;
using NewsletterAPI.Common.Enums;
using NewsletterAPI.DAL.DataFactory;
using NewsletterAPI.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NewsletterAPI.BLL.Services.NewsletterService
{
    public class NewsletterUserSubscribeService : INewsletterUserSubscribeService
    {
        private readonly INewsletterRepository _newsletterRepository;
        private readonly IMailService _mailingService;
        private readonly ILogger<NewsletterUserSubscribeService> _logger;

        public NewsletterUserSubscribeService(INewsletterRepository newsletterRepository, IMailService mailingService, ILogger<NewsletterUserSubscribeService> logger)
        {
            _newsletterRepository = newsletterRepository;
            _mailingService = mailingService;
            _logger = logger;
        }

        //First step when the visitor register their email for newsletter
        public async Task<ResponseCode> RegisterUserAsync(string email)
        {
            //Does this already exists?
            if (await _newsletterRepository.GetNewsletterUserAsync(email) != null)
                return ResponseCode.Conflict;

            NewsletterUser user = new()
            {
                Email = email,
                VerificationCode = CreateEmailVerificationCode(),
                IsVerified = false,
                CreatedDate = DateTime.Now,
                ExpiredDate = DateTime.Now.AddMinutes(15)
            };

            if (await _newsletterRepository.AddNewsletterUserAsync(user) == false)
                return ResponseCode.ServerError;

            await _mailingService.SendVerificationCodeAsync(user);
            return ResponseCode.Success;
        }

        public string CreateEmailVerificationCode()
        {
            Random random = new();
            string alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUV";
            int length = 12;
            int i = 0;
            string randomCode = "";

            while (i < length)
            {
                randomCode += alpha.ElementAt(random.Next(alpha.Length));
                i++;
            }

            return randomCode;
        }

        public async Task<ResponseCode> VerifyCode(string email, string verificationCode)
        {
            NewsletterUser user = await _newsletterRepository.GetNewsletterUserAsync(email);

            if (user is null) return ResponseCode.NotFound;

            if (!user.VerificationCode.Equals(verificationCode) || DateTime.Now > user.ExpiredDate.AddMinutes(15))
                return ResponseCode.RequestTimeout;

            user.IsVerified = true;
            user.VerfiedDate = DateTime.Now;

            if (await _newsletterRepository.UpdateNewsletterUserAsync(user))
            {
                await _mailingService.SendNewsletterConfirmation(user);
                return ResponseCode.Success;
            }

            return ResponseCode.ServerError;
        }
    }
}
