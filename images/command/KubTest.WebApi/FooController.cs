using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KubTest.EventSourcing;
using KubTest.Model;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace KubTest.WebApi
{
    [Route("api/[controller]")]
    public class FooController : Controller
    {
        private readonly IRepository<Foo> _fooRepository;
        private readonly IActionDescriptorCollectionProvider _provider;

        public FooController(IRepository<Foo> fooRepository)
        {
            _fooRepository = fooRepository ?? throw new ArgumentNullException(nameof(fooRepository));
        }

        [HttpGet("routes")]
        public IActionResult GetRoutes()
        {
            var routes = _provider.ActionDescriptors.Items.Select(x => new {
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo.Name,
                Template = x.AttributeRouteInfo.Template
            }).ToList();
            return Ok(routes);
        }

        [HttpGet("{id}", Name = "FooLink")]
        public IActionResult Get(Guid id)
        {
            var foo = _fooRepository.GetById(id);
            if (foo == null) return NotFound();                
            return Ok(foo);
        }

        public IActionResult CreateFoo([FromBody]string color)
        {
            var id = Guid.NewGuid();
            var foo = Foo.Create(id, color);
            _fooRepository.Save(foo);
            return CreatedAtRoute("FooLink", new { id = foo.Id }, foo);
        }

        [HttpPost("{id:command(NoOp)}")]
        public IActionResult NoOp(Guid id, [FromBody]string color)
        {
            return Ok();
        }

        [HttpPost("{id:command(ChangeColor)}")]
        public IActionResult ChangeColor(Guid id, [FromBody]string color)
        {            
            var foo = _fooRepository.GetById(id);
            if (foo == null) return NotFound();
            foo.ChangeColor(color);
            _fooRepository.Save(foo);
            return Ok();
        }
    }
}
