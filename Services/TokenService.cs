using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Crud_application.Models;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config)
    {
        _config = config;
    }

    public string CreateToken(Register user)
    {
        var claims = new List <Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId",user.Id.ToString())
        };
        var Permissions = GetPermissionsByRole(user.Role);

        foreach (var permission in Permissions)
        {
            claims.Add(new Claim("Permission", permission));
        }
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"])
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<string> GetPermissionsByRole(string role)
    {
        return role switch
        {
            "Admin" => new List<string>
            {
                "Student.View",
                "Student.Create",
                "Student.Edit",
                "Student.Delete"
            },

            "Teacher" => new List<string>
            {
                "Student.View",
                "Student.Create",
                "Student.Edit"
            },

            "Student" => new List<string>
            {
                "Student.View"
            },

            _ => new List<string>()
        };
    }
}
