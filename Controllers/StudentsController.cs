using Cassandra;
using CassandraAPI.Models;
using CassandraAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CassandraAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly Cassandra.ISession _session;

        public StudentsController(CassandraService cassandraService)
        {
            _session = cassandraService.GetSession();
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var rs = _session.Execute("SELECT * FROM students");

            var students = rs.Select(row => new Student
            {
                Id = row.GetValue<Guid>("id"),
                Name = row.GetValue<string>("name"),
                Career = row.GetValue<string>("career")
            });

            return Ok(students);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            var query = new SimpleStatement(
                "INSERT INTO students (id, name, career) VALUES (?, ?, ?)",
                Guid.NewGuid(),
                student.Name,
                student.Career
            );

            _session.Execute(query);

            return Ok(new
            {
                message = "Estudiante agregado"
            });
        }
    }
}