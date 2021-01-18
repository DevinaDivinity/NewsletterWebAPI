using NewsletterAPI.Entities;
using System.Threading.Tasks;

namespace NewsletterAPI.DAL.DataFactory
{
    public interface INewsletterRepository
    {
        public Task<NewsletterUser> GetNewsletterUserAsync(string email);
        public Task<bool> AddNewsletterUserAsync(NewsletterUser user);
        public Task<bool> UpdateNewsletterUserAsync(NewsletterUser user);
    }
}
