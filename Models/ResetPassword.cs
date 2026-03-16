namespace Curd_application.Models;

public class ResetPassword
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string NewPassword { get; set; }
}
