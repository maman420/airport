using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.Hubs
{
    public interface IairportHub
    {
        Task SendData(string allFlights);
    }
}