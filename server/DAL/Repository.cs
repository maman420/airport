using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;

namespace server.DAL
{
    public class Repository : IRepository
    {
        private readonly DataContext _context;
        
        public Repository(DataContext context)
        {
            _context = context;
        }
        
        public async Task DeleteAll()
        {
            _context.flights.RemoveRange(_context.flights);
            await _context.SaveChangesAsync();
        }
        
        public async Task<int> DeleteFlight(int id)
        {
            var flight = _context.flights.FirstOrDefault(f => f.Id == id);
            if (flight == null)
            {
                return 0;
            }
            
            _context.flights.Remove(flight);
            await _context.SaveChangesAsync();
            return 1;
        }
        
        public async Task<Flight?> FindFlight(int id)
        {
            return await Task.FromResult(_context.flights.FirstOrDefault(f => f.Id == id));
        }
        
        public IEnumerable<Flight> GetAll()
        {
            return _context.flights;
        }
        
        public async Task AddFlight(Flight flight)
        {
            _context.flights.Add(flight);
            await _context.SaveChangesAsync();
        }
        
        public IEnumerable<Flight> AllFlightsInLeg(int legNum)
        {
            return _context.flights.Where(flight => flight.LegLocation == legNum);
        }
        
        public bool isPlanesInLeg(int legNum)
        {
            return _context.flights.Any(flight => flight.LegLocation == legNum);
        }
        
        public async Task ChangeLeg(int flightId, int newLegNum)
        {
            var changeFlightLeg = await FindFlight(flightId);
            if (changeFlightLeg != null)
            {
                changeFlightLeg.LegLocation = newLegNum;
                await _context.SaveChangesAsync();
                
                if (newLegNum == 0)
                {
                    _context.flightsLogger.Add(changeFlightLeg);
                    _context.flights.Remove(changeFlightLeg);
                    await _context.SaveChangesAsync();
                }
            }
        }
        
        public int howMuchInLeg(int legNum)
        {
            return _context.flights.Count(flight => flight.LegLocation == legNum);
        }
    }
}
