using Crud_application.Models;
using Curd_application.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_application.Data;

public class AppDbContext : DbContext

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Register> Registers { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<ResetPassword> resets { get; set; }
    public DbSet<Employee> Employees { get; set; }
}
