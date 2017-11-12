using System;
using Microsoft.AspNetCore.Mvc;
using KubTest.EventSourcing;
using KubTest.Model;

namespace KubTest.WebApi
{
    [Route("api/[controller]")]
    public class FooController : Controller
    {
        private readonly IRepository<Foo> _fooRepository;

        public FooController(IRepository<Foo> fooRepository)
        {
            _fooRepository = fooRepository ?? throw new ArgumentNullException(nameof(fooRepository));
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
