using System;

namespace MovieAdvice.Model.Entities
{
    public class UserMovieVote : BaseEntity
    {
        public int MovieId { get; set; }

        public Movie Movie { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int VoteValue { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
