using Crud_application.Data;
using Curd_application.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Curd_application.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAll();
        Task<Employee> Create(Employee emp);
        Task<Employee?> Update(int id, Employee emp);
        Task<bool> Delete(int id);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<EmployeeHub> _hub;

        public EmployeeService(AppDbContext context, IHubContext<EmployeeHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _context.Employees.ToListAsync();
        }

        //public async Task<Employee> Create(Employee emp)
        //{
        //    emp.Id = 0;
        //    _context.Employees.Add(emp);
        //    await _context.SaveChangesAsync();

        //    await _hub.Clients.All.SendAsync("ReceiveUpdate", "Created", emp);

        //    return emp;
        //}
        public async Task<Employee> Create(Employee emp)
        {
            var newEmp = new Employee
            {
                Name = emp.Name,
                Role = emp.Role,
                Department = emp.Department,
                Salary = emp.Salary
            };

            _context.Employees.Add(newEmp);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveUpdate", "Created", newEmp);

            return newEmp;
        }

        public async Task<Employee?> Update(int id, Employee emp)
        {
            var data = await _context.Employees.FindAsync(id);
            if (data == null) return null;

            data.Name = emp.Name;
            data.Role = emp.Role;
            data.Department = emp.Department;
            data.Salary = emp.Salary;

            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveUpdate", "Updated", data);

            return data;
        }

        public async Task<bool> Delete(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;

            _context.Employees.Remove(emp);
            await _context.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("ReceiveUpdate", "Deleted", emp);

            return true;
        }
    }
}