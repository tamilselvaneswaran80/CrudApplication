using Crud_application.Models;
using Crud_application.Services;
using Curd_application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crud_application.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 🔥 Authentication required
    public class CrudController : ControllerBase
    {
        private readonly ICrudService _crudService;

        public CrudController(ICrudService crudService)
        {
            _crudService = crudService;
        }
        // CREATE
        // REGISTER
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] Register user)
        {
            var result = await _crudService.Register(user);
            return Ok(result);
        }

        // LOGIN
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] User login)
        {
            var result = await _crudService.Login(login);

            if (result == null)
                return Unauthorized("Invalid Email or Password");

            return Ok(result);
        }

        // UPDATE USER
        [HttpPut("update/{id}")]
        [Authorize(Policy = "Student.Edit")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Register user)
        {
            if (user == null)
                return BadRequest("User data is empty");
            var result = await _crudService.Update(id, user);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET USER BY ID       
        [HttpGet("users/{id}")]
        [Authorize(Policy = "Student.View")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _crudService.GetUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // DELETE USER
        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "Student.Delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _crudService.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPut("reset-password/{id}")]
        [Authorize(Policy = "Student.Edit")]
        public async Task<IActionResult> ResetPassword(int id, ResetPassword reset)
        {
            var result = await _crudService.ResetPassword(id,reset);

            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok(new { message = "Password updated successfully" });
        }
        [HttpGet("get")]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var users = await _crudService.GetUsers();
            return Ok(users);
        }
    }
}
