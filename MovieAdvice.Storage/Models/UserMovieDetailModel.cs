using System;
using System.Collections.Generic;

namespace MovieAdvice.Storage.Models
{
    public class UserMovieDetailModel
    {
        public int Id { get; set; }
        public bool IsAdult { get; set; }
        public string BackdropPath { get; set; }
        public string OriginalLanguage { get; set; }
        public string OriginalTitle { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Title { get; set; }
        public bool IsVideo { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserRate { get; set; }
        public double AverageRate { get; set; }
        public List<UserMovieNoteModel> Notes { get; set; }
    }

    public class UserMovieNoteModel
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

