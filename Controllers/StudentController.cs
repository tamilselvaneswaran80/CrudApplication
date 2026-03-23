using Crud_application.Services;
using Curd_application.Models;
using Microsoft.AspNetCore.Mvc;
//using static Curd_application.Services.StudentService;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudents();
            return Ok(students);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            var student = await _studentService.GetStudent(id);

            if (student == null)
                return NotFound();

            return Ok(student);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {

            if (student == null)
                return BadRequest("Invalid data");

            var result = await _studentService.CreateStudent(student);
            return Ok(result);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            var result = await _studentService.UpdateStudent(id, student);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var deleted = await _studentService.DeleteStudent(id);

            if (!deleted)
                return NotFound();

            return Ok(new { message = "Student Deleted" });
        }
    }
}