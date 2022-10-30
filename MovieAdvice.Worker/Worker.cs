using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieAdvice.Infrastructure.Configurations;
using MovieAdvice.Service.Services;
using MovieAdvice.Storage.Models;
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
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
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

                await Task.Delay(TimeSpan.FromMinutes(WorkerConfiguration.SchedulePeriod), stoppingToken);
            }
        }


        private async Task<MovieApiResultModel> GetMoviesByPage(int page)
        {
            using var client = new HttpClient();
            {
                client.BaseAddress = new Uri(WorkerConfiguration.TheMovieApi_BaseUrl);

                var result = await client.GetAsync($"/3/discover/movie?api_key={WorkerConfiguration.TheMovieApi_ApiKey}&page={page}");

                var json = await result.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<MovieApiResultModel>(json);

                return response;
            }
        }
    }
}