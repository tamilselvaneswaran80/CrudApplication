using Curd_application.Models;
using Microsoft.AspNetCore.SignalR;

public class EmployeeHub : Hub
{
    public async Task SendUpdate(string action, Employee emp)
    {
        await Clients.All.SendAsync("ReceiveUpdate", action, emp);
    }
}