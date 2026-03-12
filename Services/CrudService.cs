using Crud_application.Data;
using Crud_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_application.Services
{
    public interface ICrudService
    {
        Task<Register> Register(Register user);
        Task<string> Login(User login);
        Task<Register?> Update(int id, Register user);
        Task<bool> Delete(int id);
        Task<Register?> GetUser(int id);
    }

    public class CrudService : ICrudService
    {

        private readonly TokenService _tokenService;
        private readonly AppDbContext _context;

        public CrudService(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // REGISTER
        public async Task<Register> Register(Register user)
        {
            _context.Registers.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        // LOGIN
        public async Task<string> Login(User login)
        {
            var user = await _context.Registers
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            var token = _tokenService.CreateToken(user.Email);

            return token;
        }

        // UPDATE
        public async Task<Register?> Update(int id, Register user)
        {
            var existing = await _context.Registers.FindAsync(id);

            if (existing == null)
                return null;

            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.PhoneNumber = user.PhoneNumber;
            existing.Email = user.Email;
            existing.Password = user.Password;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE
        public async Task<bool> Delete(int id)
        {
            var user = await _context.Registers.FindAsync(id);

            if (user == null)
                return false;

            _context.Registers.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

        // GET USER BY ID
        public async Task<Register?> GetUser(int id)
        {
            try
            {
                var user = await _context.Registers.FindAsync(id);
                return user;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

                return null;

            }
        }
    }
}
