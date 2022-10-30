using Microsoft.AspNetCore.Mvc;
using MovieAdvice.Service.Services;
using MovieAdvice.Storage.Models;
using System.Threading.Tasks;

namespace MovieAdvice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Kullanıcıların login işlemini gerçekleştirir.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            var result = await _userService.Authenticate(model);
            return Ok(result);
        }
    }
}
