using System;

namespace MovieAdvice.Storage.Entities
{
    public class UserMovieRate : BaseEntity
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int Rate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
