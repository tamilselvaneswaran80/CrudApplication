using System.ComponentModel.DataAnnotations;

namespace Curd_application.Models;

public class Student
{
    [Key]
    public int StudentId { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public int Age { get; set; }

    public string Course { get; set; }

    public string PhoneNumber { get; set; }
}
