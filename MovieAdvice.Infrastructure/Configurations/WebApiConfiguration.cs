using Microsoft.Extensions.Configuration;
using MovieAdvice.Storage.Models;

namespace MovieAdvice.Infrastructure.Configurations
{
    public static class WebApiConfiguration
    {
        public static string Secret { get; set; }
        public static RabbitMQSettingModel RabbitMQSettings { get; set; }        

        public static void Initialize(IConfiguration configuration)
        {
            Secret = configuration["WebsiteParameters:Secret"];
            bool.TryParse(configuration["RabbitMQSettings:Enabled"], out bool rabbitMQEnabled);

            RabbitMQSettings = new RabbitMQSettingModel
            {
                Enabled = rabbitMQEnabled,
                HostName = configuration["RabbitMQSettings:HostName"],
                Password = configuration["RabbitMQSettings:Password"],
                UserName = configuration["RabbitMQSettings:Password"],
            };
        }
    }
}
