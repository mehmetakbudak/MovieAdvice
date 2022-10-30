using Microsoft.EntityFrameworkCore;
using MovieAdvice.Infrastructure;
using MovieAdvice.Storage.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IUserRepository
    {
        User GetById(int id);
        Task<User> GetUser(string email, string password);
    }

    public class UserRepository: IUserRepository
    {
        private readonly MovieAdviceContext _context;

        public UserRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id && x.IsActive);
        }

        public async Task<User> GetUser(string email, string password)
        {
            var hashedPassword = SecurityHelper.Sha256Hash(password);
            return  await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Email == email && x.Password == hashedPassword);
        }
    }
}
