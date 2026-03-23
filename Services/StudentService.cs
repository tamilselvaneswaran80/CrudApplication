using Crud_application.Data;
using Curd_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_application.Services
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetStudents();
        Task<Student?> GetStudent(int id);
        Task<Student> CreateStudent(Student student);
        Task<Student?> UpdateStudent(int id, Student student);
        Task<bool> DeleteStudent(int id);
    }
    public class StudentService : IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetStudents()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetStudent(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateStudent(Student student)
        {
            student.StudentId = 0;

            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateStudent(int id, Student student)
        {
            var existingStudent = await _context.Students.FindAsync(id);

            if (existingStudent == null)
                return null;

            existingStudent.Firstname = student.Firstname;
            existingStudent.Lastname = student.Lastname;
            existingStudent.Age = student.Age;
            existingStudent.Course = student.Course;
            existingStudent.PhoneNumber = student.PhoneNumber;

            await _context.SaveChangesAsync();

            return existingStudent;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}