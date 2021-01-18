using System.ComponentModel.DataAnnotations;

namespace NewsletterAPI.Models
{
    public class AuthModel
    {
        [Required]
        public string ClientId { get; internal set; }
        [Required]
        public string SecretKey { get; internal set; }
    }
}
