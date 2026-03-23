using Curd_application.Models;
using Curd_application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Curd_application.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeservice;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeservice = employeeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeeservice.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            var result = await _employeeservice.Create(emp);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Employee emp)
        {
            var result = await _employeeservice.Update(id, emp);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeservice.Delete(id);
            if (!result)
                return NotFound("Employee not found"); // 🔥 proper 404
            return Ok(result);
        }
    }
}

