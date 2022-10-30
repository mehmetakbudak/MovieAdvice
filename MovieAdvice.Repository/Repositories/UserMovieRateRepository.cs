using Microsoft.EntityFrameworkCore;
using MovieAdvice.Storage.Entities;
using MovieAdvice.Storage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieAdvice.Repository.Repositories
{
    public interface IUserMovieRateRepository
    {
        Task<List<UserMovieRate>> GetMovieRates(int movieId);
        Task<ServiceResult> CreateOrUpdate(MovieRateModel model, int userId);
    }

    public class UserMovieRateRepository : IUserMovieRateRepository
    {
        private readonly MovieAdviceContext _context;

        public UserMovieRateRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public async Task<List<UserMovieRate>> GetMovieRates(int movieId)
        {
            return await _context.UserMovieRates
                .Where(x => x.MovieId == movieId).ToListAsync();
        }

        public async Task<ServiceResult> CreateOrUpdate(MovieRateModel model, int userId)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userMovieRate = await _context.UserMovieRates
                .FirstOrDefaultAsync(x => x.MovieId == model.MovieId && x.UserId == userId);

            if (userMovieRate == null)
            {
                await _context.UserMovieRates.AddAsync(new UserMovieRate
                {
                    CreatedDate = DateTime.Now,
                    MovieId = model.MovieId,
                    UserId = userId,
                    Rate = model.Rate
                });
            }
            else
            {
                userMovieRate.Rate = model.Rate;
                _context.UserMovieRates.Update(userMovieRate);
            }

            return result;
        }
    }
}
