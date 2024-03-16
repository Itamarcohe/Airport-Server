using AirportServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace AirportServer.Hubs
{
    public class AirportHub : Hub
    {
        public async Task UpdateLogs(Log log)
        {
            await Clients.All.SendAsync("UpdateLogs", log);
        }
    }
}
