using MovieAdvice.Infrastructure;
using MovieAdvice.Infrastructure.Exceptions;
using MovieAdvice.Model.Entities;
using MovieAdvice.Model.Models;
using MovieAdvice.Repository.Repositories;
using System.Net;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Services
{
    public interface IUserService
    {
        Task<ServiceResult> Authenticate(LoginModel model);
        Task<User> GetById(int id);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHelper _jwtHelper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IJwtHelper jwtHelper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Authenticate(LoginModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

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



            return result;
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }
    }
}
