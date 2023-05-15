using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using server.Hubs;
using server.Models;

namespace server.DAL
{
    public class DataContext : DbContext
    {
        private IHubContext < airportHub, IairportHub > _airportHub;
        public DataContext(DbContextOptions options, IHubContext < airportHub, IairportHub > airportHub) : base(options)
        {
            _airportHub = airportHub;
        }   
        public DbSet<Flight> flights { get; set; }
        public DbSet<Flight> flightsLogger { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)        
        {
            var result = base.SaveChangesAsync();

            IEnumerable<Flight> allFlights = flights.ToList();
            string allFlightsJson = JsonConvert.SerializeObject(allFlights);
            _airportHub.Clients.All.SendAllFlights(allFlightsJson);

            return result;
        }

    }
}