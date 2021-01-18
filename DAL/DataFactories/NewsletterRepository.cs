using Microsoft.EntityFrameworkCore;
using NewsletterAPI.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace NewsletterAPI.DAL.DataFactory
{
    public class NewsletterRepository : INewsletterRepository
    {
        DataContext _dataContext;

        public NewsletterRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<NewsletterUser> GetNewsletterUserAsync(string email)
        {
            return await _dataContext.NewsletterUsers.Where(user => user.Email.Equals(email)).FirstOrDefaultAsync();
        }


        public async Task<bool> AddNewsletterUserAsync(NewsletterUser user)
        {
            try
            {
                _dataContext.NewsletterUsers.Add(user);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateNewsletterUserAsync(NewsletterUser user)
        {
            try
            {
                _dataContext.NewsletterUsers.Update(user);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
