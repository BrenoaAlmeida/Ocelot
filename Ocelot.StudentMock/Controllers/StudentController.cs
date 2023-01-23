using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Ocelot.StudentMock.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        [HttpGet(Name = "GetStudent")]
        public List<object> Get()
        {
            var students = new List<object>
            {
                new { Id = 1, Name = "Student 1", Gender = "M" },
                new { Id = 1, Name = "Student 2", Gender = "F" },
                new { Id = 3, Name = "Student 3", Gender = "F" }
            };
            return students;
        }
    }
}