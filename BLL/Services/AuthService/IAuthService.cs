using Microsoft.Extensions.Configuration;
using NewsletterAPI.Models;
using System.Threading.Tasks;

namespace NewsletterAPI.BLL.Services.AuthService
{
    public interface IAuthService
    {
        public Task<AuthModel> Authenticate(string clientId, string secretKey);
    }

    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;

        public AuthService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<AuthModel> Authenticate(string clientId, string secretKey)
        {
            if (_config.GetValue<string>("Auth:CliendId").Equals(clientId) && _config.GetValue<string>("Auth:SecretKey").Equals(secretKey))
            {
                return new AuthModel()
                {
                    ClientId = clientId,
                    SecretKey = secretKey
                };
            }

            return null;
        }
    }
}
