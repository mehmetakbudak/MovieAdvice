using MovieAdvice.Storage.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading;

namespace MovieAdvice.Service.Services
{
    public interface IRabbitMQService
    {
        IConnection GetConnection(RabbitMQSettingModel model);
        IModel GetModel(IConnection connection);
    }

    public class RabbitMQService : IRabbitMQService
    {
        public IConnection GetConnection(RabbitMQSettingModel model)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = model.HostName,
                    UserName = model.UserName,
                    Password = model.Password,
                };

                // Otomatik bağlantı kurtarmayı etkinleştirmek için,
                factory.AutomaticRecoveryEnabled = true;
                // Her 10 sn de bir tekrar bağlantı toparlanmaya çalışır 
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                // sunucudan bağlantısı kesildikten sonra kuyruktaki mesaj tüketimini sürdürmez 
                // (TopologyRecoveryEnabled = false   olarak tanımlandığı için)
                factory.TopologyRecoveryEnabled = false;

                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                // loglama işlemi yapabiliriz
                Thread.Sleep(5000);
                // farklı business ta yapılabilir, ancak biz tekrar bağlantı (connection) kurmayı deneyeceğiz
                return GetConnection(model);
            }
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
