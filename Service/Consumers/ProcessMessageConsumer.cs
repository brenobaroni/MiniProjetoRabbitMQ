using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Consumers.Options;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Data.Models;

namespace Service.Consumers
{
    public class ProcessMessageConsumer : BackgroundService
    {
        private readonly RabbitMqConfiguration _rabbitMqConfiguration;
        private readonly IConnection _connectionChannel;
        private readonly IModel _channel;

        public ProcessMessageConsumer(IOptions<RabbitMqConfiguration> options, IServiceProvider serviceProvider)
        {
            _rabbitMqConfiguration = options.Value;
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfiguration.Host
            };

            _connectionChannel = connectionFactory.CreateConnection();
            _channel = _connectionChannel.CreateModel();
            _channel.QueueDeclare(
                queue: _rabbitMqConfiguration.Queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var contentArray = eventArgs.Body.ToArray();
                var contentString = Encoding.UTF8.GetString(contentArray);
                var message = JsonConvert.DeserializeObject<MessageInputModel>(contentString);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(_rabbitMqConfiguration.Queue, false, consumer);

            return Task.CompletedTask;
        }
    }
}
