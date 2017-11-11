using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using KubTest.EventSourcing;

namespace KubTest.WebApi
{
    public class RabbitMQEventPublisher : IEventPublisher
    {
        private readonly RabbitMQOptions _options;
        private readonly ILogger _logger;

        public RabbitMQEventPublisher(IOptions<RabbitMQOptions> optionsAccessor, ILogger<RabbitMQEventPublisher> logger)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }

		public void PublishEventRecord(IEventRecord eventRecord)
		{
            _logger.LogDebug("Publishing message to: " + _options.ConnectionString);
            _logger.LogDebug("Echange Name: " + _options.Exchange ?? "<null>");

            var routingKey = string.Format("{0}.{1}", eventRecord.EventType, eventRecord.ModelId.ToString("N"));
            var message = JsonConvert.SerializeObject(eventRecord);
            _logger.LogDebug("Message: " + message);

            var body = Encoding.UTF8.GetBytes(message);

            var factory = new ConnectionFactory() { Uri = new Uri(_options.ConnectionString) };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(_options.Exchange, ExchangeType.Topic);
                channel.BasicPublish(exchange: _options.Exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }

			_logger.LogDebug(string.Format("{0} on {1} published", eventRecord.EventType, eventRecord.ModelId));
		}
    }
}