using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MovieAdvice.Storage.Models
{
    public class MovieApiResultModel
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("results")]
        public List<MovieApiItemModel> Movies { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }

    public class MovieApiItemModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("adult")]
        public bool IsAdult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("release_date")]
        public DateTime? ReleaseDate { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("video")]
        public bool IsVideo { get; set; }

        [JsonPropertyName("vote_average")]
        public double RateAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int RateCount { get; set; }
    }
}
