using Microsoft.AspNetCore.Http;
using MovieAdvice.Infrastructure;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Repository.Repositories;
using MovieAdvice.Storage.Entities;
using MovieAdvice.Storage.Models;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Services
{
    public interface IUserService
    {
        User GetById(int id);
        Task<TokenReponseModel> Authenticate(LoginModel model);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserRepository userRepository,
            IJwtHelper jwtHelper,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenReponseModel> Authenticate(LoginModel model)
        {
            if (model == null)
            {
                throw new BadRequestException("model null olamaz.");
            }

            var user = await _userRepository.GetUser(model.Email, model.Password);

            if (user == null)
            {
                throw new NotFoundException("Email adresi veya şifre hatalıdır.");
            }

            if (!user.IsActive)
            {
                throw new BadRequestException("Hesabınız aktif değildir.");
            }

            var jwtToken = _jwtHelper.GenerateJwtToken(user);

            user.Token = jwtToken.Token;
            user.TokenExpireDate = jwtToken.ExpireDate;

            await _unitOfWork.SaveAsync();

            var result = new TokenReponseModel()
            {
                UserId = user.Id,
                Token = jwtToken.Token,
                NameSurname = $"{user.FirstName} {user.LastName}",
                TokenExpireDate = jwtToken.ExpireDate
            };

            return result;
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }
    }
}
