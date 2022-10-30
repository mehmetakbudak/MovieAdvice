using Microsoft.Extensions.Configuration;
using MovieAdvice.Storage.Models;

namespace MovieAdvice.Infrastructure.Configurations
{
    public static class ConsumerConfiguration
    {
        public static RabbitMQSettingModel RabbitMQSettings { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            RabbitMQSettings = new RabbitMQSettingModel
            {
                Enabled = true,
                HostName = configuration["RabbitMQSettings:HostName"],
                Password = configuration["RabbitMQSettings:Password"],
                UserName = configuration["RabbitMQSettings:UserName"]
            };
        }
    }
}
