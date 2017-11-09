using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using command.Data;
using command.Model;

namespace command.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IRepository<Foo> _fooRepository;
        private readonly ILogger _logger;

        public ValuesController(IRepository<Foo> fooRepository, ILogger<ValuesController> logger)
        {
            if (fooRepository == null)
                throw new ArgumentNullException("fooRepository");

            _fooRepository = fooRepository;
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _logger.LogDebug("Entering values get");
            try
            {
                var foo = _fooRepository.GetById(Guid.NewGuid());
                _logger.LogDebug("Model color: " + foo.Color);
                foo.ChangeColor("green");
                _fooRepository.Save(foo);
                _logger.LogDebug("Model color: " + foo.Color);
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
    }
}
