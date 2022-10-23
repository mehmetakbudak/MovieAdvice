using MovieAdvice.Infrastructure;
using MovieAdvice.Model.Entities;
using MovieAdvice.Model.Models;
using MovieAdvice.Repository.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Services
{
    public interface IMovieService
    {
        PagedResponse<List<Movie>> GetByPagination(PaginationFilter filter);
        Task AddRange(List<MovieApiItemModel> list);
    }

    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MovieService(IMovieRepository movieRepository,
            IUnitOfWork unitOfWork)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
        }

        public PagedResponse<List<Movie>> GetByPagination(PaginationFilter filter)
        {
            var list = _movieRepository.GetAll();

            var pagedReponse = PaginationHelper.CreatePagedReponse<Movie>(list, filter);

            return pagedReponse;
        }

        public async Task AddRange(List<MovieApiItemModel> list)
        {
            var movies = await _movieRepository.AddRange(list);
            if (movies.Count > 0)
            {
                await _unitOfWork.SaveAsync();
            }
        }
    }
}