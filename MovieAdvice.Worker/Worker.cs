using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieAdvice.Model.Models;
using MovieAdvice.Service.Services;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAdvice.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int.TryParse(_configuration["SchedulePeriod"].ToString(), out int period);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // maksimum deðeri apiden dönen TotalPages deðeri olacak þekilde random deðer üretiliyor.                
                var randomPage = new Random().Next(500);

                // random deðer sayfanýn deðeri olacak þekilde api'ye istek atýlýyor
                var pageResponse = await GetMoviesByPage(randomPage);

                if (pageResponse != null && pageResponse.Movies != null)
                {
                    using (IServiceScope scope = _serviceProvider.CreateScope())
                    {
                        var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
                        await movieService.AddRange(pageResponse.Movies);
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(period), stoppingToken);
            }
        }


        public async Task<MovieApiResultModel> GetMoviesByPage(int page)
        {
            var baseUrl = _configuration["TheMovieApiSettings:BaseUrl"].ToString();
            var apiKey = _configuration["TheMovieApiSettings:ApiKey"].ToString();

            using var client = new HttpClient();
            {
                client.BaseAddress = new Uri(baseUrl);

                var result = await client.GetAsync($"/3/discover/movie?api_key={apiKey}&page={page}");

                var json = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<MovieApiResultModel>(json);

                return response;
            }
        }
    }
}