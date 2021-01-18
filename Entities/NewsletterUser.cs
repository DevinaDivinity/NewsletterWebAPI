using System;
using System.ComponentModel.DataAnnotations;

namespace NewsletterAPI.Entities
{
    public record NewsletterUser
    {
        public int Id { get; init; }

        [Required, StringLength(300), EmailAddress]
        public string Email { get; init; }

        [StringLength(200)]
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerfiedDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime CreatedDate { get; init; }
    }
}