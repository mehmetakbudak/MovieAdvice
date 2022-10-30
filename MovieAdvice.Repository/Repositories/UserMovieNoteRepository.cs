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
    public interface IUserMovieNoteRepository
    {
        Task<List<UserMovieNote>> GetUserMovieNotes(int movieId, int userId);
        Task<ServiceResult> Add(MovieNoteModel model, int userId);
    }

    public class UserMovieNoteRepository : IUserMovieNoteRepository
    {
        private readonly MovieAdviceContext _context;

        public UserMovieNoteRepository(MovieAdviceContext context)
        {
            _context = context;
        }

        public async Task<List<UserMovieNote>> GetUserMovieNotes(int movieId, int userId)
        {
            return await _context.UserMovieNotes
                .Where(x => x.UserId == userId && x.MovieId == movieId).ToListAsync();
        }

        public async Task<ServiceResult> Add(MovieNoteModel model, int userId)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            await _context.UserMovieNotes.AddAsync(new UserMovieNote
            {
                UserId = userId,
                CreatedDate = DateTime.Now,
                MovieId = model.MovieId,
                Note = model.Note
            });

            return result;
        }
    }
}
