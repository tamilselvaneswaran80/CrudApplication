namespace Curd_application.Models;

public class Permissions
{

    public const string Student_View = "Student.View";

    public const string Student_Create = "Student.Create";

    public const string Student_Edit = "Student.Edit";

    public const string Student_Delete = "Student.Delete";
}

public static class RolePermissions
{
    public static Dictionary<string, List<string>> Permissions = new()
    {
        { "Admin", new List<string>
            {
                "Student.View",
                "Student.Create",
                "Student.Edit",
                "Student.Delete"
            }
        },
        { "Teacher", new List<string>
            {
                "Student.View",
                "Student.Create",
                "Student.Edit"
            }
        },
        { "Student", new List<string>
            {
                "Student.View"
            }
        }
    };
}