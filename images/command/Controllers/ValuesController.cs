using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace command.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly RabbitMQOptions _options;
        private readonly ILogger _logger;

        public ValuesController(IOptions<RabbitMQOptions> optionsAccessor, ILogger<ValuesController> logger)
        {
            _options = optionsAccessor.Value;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug("Entering values get");
            try
            {
                PublishMessage();
            }
            catch (Exception e)
            {
                _logger.LogError(new EventId(-1), e, "Error publishing message");
            }

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private void PublishMessage() {
            _logger.LogDebug("Publishing message to: " + _options.ConnectionString);
            _logger.LogDebug("Echange Name: " + _options.Exchange ?? "<null>");

            var routingKey = "info";
            var message = "Hello World!";
            var body = Encoding.UTF8.GetBytes(message);

            var factory = new ConnectionFactory() { Uri = new Uri(_options.ConnectionString) };
            using(var connection = factory.CreateConnection())
            using(var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(_options.Exchange, ExchangeType.Topic, false, false, new Dictionary<string, object>());
                channel.BasicPublish(exchange: _options.Exchange,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
