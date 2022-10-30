using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieAdvice.Storage.Entities
{
    public class Movie : BaseEntity
    {
        public int UniqueId { get; set; }

        public bool IsAdult { get; set; }

        [StringLength(500)]
        public string BackdropPath { get; set; }

        [StringLength(500)]
        public string OriginalLanguage { get; set; }

        [StringLength(500)]
        public string OriginalTitle { get; set; }

        public string Overview { get; set; }

        public double Popularity { get; set; }

        [StringLength(500)]
        public string PosterPath { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [StringLength(500)]
        public string Title { get; set; }

        public bool IsVideo { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual List<UserMovieNote> UserMovieNotes { get; set; }

        public virtual List<UserMovieRate> UserMovieRates { get; set; }
    }
}
