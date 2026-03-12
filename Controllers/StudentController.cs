using Crud_application.Data;
using Curd_application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            return student;
        }

        // CREATE
        //[HttpPost]  public async Task<ActionResult<Student>> CreateStudent(Student student)
        //{
        //    _context.Students.Add(student);
        //    await _context.SaveChangesAsync();

        //    return Ok(student);
        //}


        // CREATE
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Student data is null");
                }

                await _context.Students.AddAsync(student);
                await _context.SaveChangesAsync();

                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            try
            {
                var existingStudent = await _context.Students.FindAsync(id);

                if (existingStudent == null)
                {
                    return NotFound("Student not found");
                }

                // Update fields
                existingStudent.Firstname = student.Firstname;
                existingStudent.Lastname = student.Lastname;
                existingStudent.Age = student.Age;
                existingStudent.Course = student.Course;
                existingStudent.PhoneNumber = student.PhoneNumber;

                await _context.SaveChangesAsync();

                return Ok(existingStudent);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // UPDATE
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateStudent(int id, Student student)
        //{
        //    if (id != student.StudentId)

        //        return BadRequest();

        //    _context.Entry(student).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return NotFound();

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
