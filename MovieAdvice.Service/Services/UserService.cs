using MovieAdvice.Repository.Repositories;

namespace MovieAdvice.Service.Services
{
    public interface IUserService
    {

    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
