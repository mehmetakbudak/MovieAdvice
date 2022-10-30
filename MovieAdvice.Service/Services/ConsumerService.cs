using MovieAdvice.Infrastructure;
using MovieAdvice.Storage.Consts;
using MovieAdvice.Storage.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MovieAdvice.Service.Services
{
    public interface IConsumerService
    {
        Task Start(RabbitMQSettingModel settings);
    }

    public class ConsumerService : IConsumerService
    {    
        private EventingBasicConsumer _consumer;
        private IModel _channel;
        private IConnection _connection;

        private readonly IRabbitMQService _rabbitMQServices;
        private readonly IObjectConvertFormat _objectConvertFormat;
        private readonly ISendMailService _sendMailService;

        public ConsumerService(
            IRabbitMQService rabbitMQServices,
            IObjectConvertFormat objectConvertFormat,
            ISendMailService sendMailService)
        {
            _rabbitMQServices = rabbitMQServices;
            _sendMailService = sendMailService ?? throw new ArgumentNullException(nameof(sendMailService));
            _objectConvertFormat = objectConvertFormat;
        }

        public async Task Start(RabbitMQSettingModel settings)
        {
            try
            {
                _connection = _rabbitMQServices.GetConnection(settings);
                _channel = _rabbitMQServices.GetModel(_connection);
                _channel.QueueDeclare(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                _channel.BasicQos(0, RabbitMQConsts.ParallelThreadsCount, false);
                _consumer = new EventingBasicConsumer(_channel);
                _channel.BasicConsume(queue: RabbitMQConsts.RabbitMqConstsList.QueueNameEmail.ToString(),
                                           autoAck: false,
                                           /* autoAck: bir mesajı aldıktan sonra bunu anladığına       
                                              dair(acknowledgment) kuyruğa bildirimde bulunur ya da timeout gibi vakalar oluştuğunda 
                                              mesajı geri çevirmek(Discard) veya yeniden kuyruğa aldırmak(Re-Queue) için dönüşler yapar*/
                                           consumer: _consumer);

                _consumer.Received += async (sender, e) =>
                {
                    MailModel message = _objectConvertFormat.JsonToObject<MailModel>(Encoding.UTF8.GetString(e.Body.ToArray()));
                    var task = await _sendMailService.Send(message);
                    _channel.BasicAck(e.DeliveryTag, false);
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        
    }
}
