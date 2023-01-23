using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ocelot.PersonMock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<PersonController> _logger;

        public PersonController(ILogger<PersonController> logger)
        {
            _logger = logger;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet(Name = "GetPerson")]
        public List<object> Get()
        {
            var person = new List<object>
            {
                new { Id = 1, Name = "Person 1", Gender = "M" },
                new { Id = 1, Name = "Person 2", Gender = "F" },
                new { Id = 3, Name = "Person 3", Gender = "F" }
            };
            return person;
        }
    }
}