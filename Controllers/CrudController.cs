using Crud_application.Models;
using Crud_application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Crud_application.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class CrudController : ControllerBase
    {
        private readonly ICrudService _crudService;

        public CrudController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] Register user)
        {
            var result = await _crudService.Register(user);
            return Ok(result);
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] User login)
        {
            var token = await _crudService.Login(login);

            if (token == null)
                return Unauthorized("Invalid Email or Password");

            return Ok(new {token});
        }

        // UPDATE USER
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Register user)
        {
            var result = await _crudService.Update(id, user);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // GET USER BY ID
        [Authorize]
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var user = await _crudService.GetUser(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var deleted = await _crudService.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
