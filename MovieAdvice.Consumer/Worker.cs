using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MovieAdvice.Infrastructure.Configurations;
using MovieAdvice.Service.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MovieAdvice.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(
            ILogger<Worker> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("consumerService alýndý.");
                _logger.LogInformation($"consumerService.Start() baþladý: {DateTime.Now.ToShortTimeString()}");

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    var consumerService = scope.ServiceProvider.GetRequiredService<IConsumerService>();
                    await consumerService.Start(ConsumerConfiguration.RabbitMQSettings);
                    _logger.LogInformation($"consumerService.Start() bitti:  {DateTime.Now.ToShortTimeString()}");
                }
            }
        }
    }
}