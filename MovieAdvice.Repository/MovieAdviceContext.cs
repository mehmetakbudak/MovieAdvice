using Microsoft.EntityFrameworkCore;
using MovieAdvice.Infrastructure;
using MovieAdvice.Storage.Entities;

namespace MovieAdvice.Repository
{
    public class MovieAdviceContext : DbContext
    {
        public MovieAdviceContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserMovieNote> UserMovieNotes { get; set; }
        public virtual DbSet<UserMovieRate> UserMovieRates { get; set; }
        public virtual DbSet<MailTemplate> MailTemplates { get; set; }
        public virtual DbSet<WebsiteParameter> WebsiteParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Deleted = false,
                    Email = "admin@admin.com",
                    FirstName = "Mehmet",
                    LastName = "Akbudak",
                    IsActive = true,
                    Password = SecurityHelper.Sha256Hash("123456")
                });
        }
    }
}
