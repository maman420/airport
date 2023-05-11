using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Hubs
{
    public class airportHub : Hub<IairportHub>
    {
        public async Task SendData(string allFlights)
        {
            await Clients.All.SendData(allFlights);
        }
    }
}