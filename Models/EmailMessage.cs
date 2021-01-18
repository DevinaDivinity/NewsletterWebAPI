namespace NewsletterAPI.Models
{
    public record EmailMessage
    {
        public string ToEmail { get; init; }
        public string Subject { get; init; }
        public string Body { get; set; }
    }
}
