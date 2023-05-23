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
    public class Repository : IRepository
    {
        private readonly object contextLock = new object();
        private readonly DataContext _context;
        public Repository(DataContext context)
        {
            _context = context;
        }
        public void DeleteAll()
        {
            lock(contextLock){
                _context.flights.RemoveRange(_context.flights);
                _context.SaveChanges();
            }

        }
        public int DeleteFlight(int id)
        {
            lock(contextLock){
                var flight = _context.flights.FirstOrDefault(f => f.Id == id);
                if (flight == null)
                {
                    return 0;
                }
                _context.flights.Remove(flight);
                _context.SaveChanges();

                return 1;
            }
        }
        public Flight FindFlight(int id)
        {
            lock(contextLock)
                return _context.flights.FirstOrDefault(f => f.Id == id);
        }
        public IEnumerable<Flight> GetAll()
        {
            lock(contextLock)
                return _context.flights.ToList();
        }
        public IEnumerable<FlightLogger> GetAllFlightLogger()
        {
            lock(contextLock)
                return _context.flightsLogger.ToList();
        }
        public void AddFlight(Flight flight)
        {
            lock (contextLock)
            {
                _context.flights.Add(flight);
                _context.SaveChanges(); 
            }                
            string flightJson = JsonConvert.SerializeObject(flight);
        }
        public void AddFlightToLogger(int flightId)
        {
            lock(contextLock){
                var flight = _context.flights.FirstOrDefault(f => f.Id == flightId);

                if(flight != null){
                    Flight flightToAdd = new Flight
                    {
                        AirLine = flight.AirLine,
                        LegLocation = flight.LegLocation,
                        Name = flight.Name
                    };
                    _context.flightsLogger.Add(new FlightLogger 
                    { 
                        Flight = flightToAdd,
                        Exit = DateTime.Now
                    });
                    _context.SaveChanges();

                }

            }
        }
        public IEnumerable<Flight> AllFlightsInLeg(int legNum)
        {
            lock (contextLock)
                return _context.flights.Where(flight => flight.LegLocation == legNum);
        }
        public bool isPlanesInLeg(int legNum)
        {
            lock(contextLock)
                return _context.flights.Any(flight => flight.LegLocation == legNum);
        }
        public void ChangeLeg(int flightId, int newLegNum)
        {
            lock(contextLock){
                var changeFlightLeg = _context.flights.FirstOrDefault(f => f.Id == flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = newLegNum;
                    _context.SaveChanges();
                }
            }
        }
        public int howMuchInLeg(int legNum)
        {
            lock(contextLock)
                return _context.flights.Count(flight => flight.LegLocation == legNum);
        }
    }
}
