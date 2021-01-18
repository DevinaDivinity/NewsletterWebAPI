using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsletterAPI.BLL.Services.NewsletterService;
using NewsletterAPI.Common.Helpers;
using NewsletterAPI.Common.Enums;

namespace NewsletterAPI.APIControllers
{
    [Route("newsletter")]
    [Authorize]
    [ApiController]
    public class NewsletterController : ControllerBase
    {
        readonly INewsletterUserSubscribeService newsletterService;

        public NewsletterController(INewsletterUserSubscribeService newsletterService)
        {
            this.newsletterService = newsletterService;
        }

        [HttpPost]
        public async Task<IActionResult> NewsletterRegister(string email)
        {
            if (!Validations.IncomingRequest(email, ValidationType.Email))
                return BadRequest("Invalid email format");

            ResponseCode statusCode = await newsletterService.RegisterUserAsync(email);

            return statusCode switch
            {
                ResponseCode.Conflict => Conflict("The user already exists"),
                ResponseCode.Success => StatusCode(201, "User is created. A mail has been sent to the user for verification."),
                ResponseCode.ServerError => StatusCode(500, "Server error.")
            };
        }

        [HttpGet]
        public async Task<IActionResult> EmailVerification(string email, string verificationCode)
        {
            if (!Validations.IncomingRequest(email, ValidationType.Email) || !Validations.IncomingRequest(verificationCode))
                return BadRequest("Invalid Request");

            ResponseCode statusCode = await newsletterService.VerifyCode(email, verificationCode);
            return statusCode switch
            {
                ResponseCode.NotFound => Conflict("Error. This user does not exists."),
                ResponseCode.RequestTimeout => Conflict("Error. The registration for newsletter has expired."),
                ResponseCode.Success => StatusCode(200, "The user is verified. A confirmation mail has been sent to user."),
                ResponseCode.ServerError => StatusCode(500, "Server error.")
            };
        }
    }
}
