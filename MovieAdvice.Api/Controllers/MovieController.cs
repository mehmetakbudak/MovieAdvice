using Microsoft.AspNetCore.Mvc;
using MovieAdvice.Infrastructure;
using MovieAdvice.Storage.Models;
using MovieAdvice.Service.Attributes;
using MovieAdvice.Service.Services;
using System.Threading.Tasks;
using MovieAdvice.Infrastructure.Extensions;

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

        /// <summary>
        /// Film listesini sayfalı olarak döner.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get([FromQuery] PaginationFilter filter)
        {
            var list = _movieService.GetByPagination(filter);
            return Ok(list);
        }

        /// <summary>
        /// Film detay bilgilerini döner.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieDetail(int id)
        {
            var result = await _movieService.GetMovieDetail(id, User.UserId());
            return Ok(result);
        }

        /// <summary>
        /// İstenilen film bilgilerini belirtilen email adresine tavsiye olarak gönderir.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Advice")]
        public async Task<IActionResult> Advice([FromBody] MovieAdviceModel model)
        {
            var result = await _movieService.Advice(model, User.UserId());
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcıların filme puan vermesini sağlar.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Rate")]
        public async Task<IActionResult> Rate([FromBody] MovieRateModel model)
        {
            var result = await _movieService.Rate(model, User.UserId());
            return Ok(result);
        }

        /// <summary>
        /// Kullanıcıların film hakkında notlarını eklemesini sağlar.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Note")]
        public async Task<IActionResult> Note([FromBody] MovieNoteModel model)
        {
            var result = await _movieService.Note(model, User.UserId());
            return Ok(result);
        }
    }
}
