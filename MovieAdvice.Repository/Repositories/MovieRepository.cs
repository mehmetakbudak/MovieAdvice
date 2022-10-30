using Microsoft.EntityFrameworkCore;
using MovieAdvice.Storage.Entities;
using MovieAdvice.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IMovieRepository
    {
        IQueryable<Movie> GetAll();
        Task<List<Movie>> AddRange(List<MovieApiItemModel> list);
        Task<Movie> GetById(int id);
        Task<Movie> GetWithRatesById(int id);
    }
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieAdviceContext _context;

        public MovieRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public IQueryable<Movie> GetAll()
        {
            return _context.Movies.AsQueryable();
        }

        public async Task<List<Movie>> AddRange(List<MovieApiItemModel> list)
        {
            var movies = new List<Movie>();

            foreach (var item in list)
            {
                var isExist = await _context.Movies.AnyAsync(x => x.UniqueId == item.Id);

                if (!isExist)
                {
                    var movie = new Movie
                    {
                        BackdropPath = item.BackdropPath,
                        CreatedDate = DateTime.Now,
                        IsAdult = item.IsAdult,
                        IsVideo = item.IsVideo,
                        OriginalLanguage = item.OriginalLanguage,
                        OriginalTitle = item.OriginalTitle,
                        Overview = item.Overview,
                        Popularity = item.Popularity,
                        PosterPath = item.PosterPath,
                        ReleaseDate = item.ReleaseDate,
                        Title = item.Title,
                        UniqueId = item.Id
                    };
                    movies.Add(movie);
                }
            }

            if (movies.Count > 0)
            {
                await _context.Movies.AddRangeAsync(movies);
            }

            return movies;
        }

        public async Task<Movie> GetById(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> GetWithRatesById(int id)
        {
            return await _context.Movies
                .Include(x => x.UserMovieRates)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}