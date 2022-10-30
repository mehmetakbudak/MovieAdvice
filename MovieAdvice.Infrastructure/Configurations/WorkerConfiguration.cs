using Microsoft.Extensions.Configuration;
using System;

namespace MovieAdvice.Infrastructure.Configurations
{
    public static class WorkerConfiguration
    {
        public static string TheMovieApi_BaseUrl { get; set; }
        public static string TheMovieApi_ApiKey { get; set; }
        public static int SchedulePeriod { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            TheMovieApi_BaseUrl = configuration["TheMovieApiSettings:BaseUrl"];
            TheMovieApi_ApiKey = configuration["TheMovieApiSettings:ApiKey"];

            Int32.TryParse(configuration["SchedulePeriod"], out int schedulePeriod);
            SchedulePeriod = schedulePeriod;
        }
    }
}
