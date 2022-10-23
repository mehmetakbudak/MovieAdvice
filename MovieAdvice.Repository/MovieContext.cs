using Microsoft.EntityFrameworkCore;
using MovieAdvice.Model.Entities;
using System.Reflection.Metadata;

namespace MovieAdvice.Repository
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMovieNote> UserMovieNotes { get; set; }
        public virtual DbSet<UserMovieVote> UserMovieVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Deleted = false,
                    Email = "admin@admin.com",
                    FirstName = "Mehmet",
                    LastName = "Akbudak",
                    IsActive = true,
                    Password = ""
                });
        }
    }
}
