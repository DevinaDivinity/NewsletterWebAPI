using NewsletterAPI.Common.Enums;
using System.Threading.Tasks;

namespace NewsletterAPI.BLL.Services.NewsletterService
{
    public interface INewsletterUserSubscribeService
    {
        public Task<ResponseCode> RegisterUserAsync(string email);
        public Task<ResponseCode> VerifyCode(string email, string verificationCode);
        string CreateEmailVerificationCode();
    }
}
