using Microsoft.AspNetCore.Mvc;
using MovieAdvice.Infrastructure;
using MovieAdvice.Service.Services;

namespace MovieAdvice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PaginationFilter filter)
        {
            var list = _movieService.GetByPagination(filter);
            return Ok(list);
        }
    }
}
