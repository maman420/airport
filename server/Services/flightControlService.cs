using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.DAL;
using server.Models;
using System.Timers;
using Timer = System.Timers.Timer;
using Microsoft.EntityFrameworkCore;

namespace server.Services
{
    public class flightControlService
    {
        private readonly DataContext _context;
        private Timer _timer;
        
        public flightControlService(DataContext context)
        {
            _context = context;

            // _timer = new Timer(1000); // 1000 milliseconds = 1 second
            // _timer.Elapsed += everySecondFunction;
            // _timer.Start();
        }
        public async void addFlight(Flight flight)
        {
            if(canEnterAirport()){
                _context.flights.Add(flight);
                await _context.SaveChangesAsync();
                leg1(flight.Id);
            } else {
                await Task.Delay(1000);
                addFlight(flight);
            }
        }
        public bool canEnterAirport()
        {
            return !_context.flights.Any(flight => flight.LegLocation == 1);
        }


        private async void leg1(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg1");
            if(_context.flights.Any(flight => flight.LegLocation == 2)) {
                await Task.Delay(1000);
                leg1(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 2;
                    await _context.SaveChangesAsync();
                }                
                leg2(flightId);
            }
        }
        private async void leg2(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg2");

            if(_context.flights.Any(flight => flight.LegLocation == 3)) {
                await Task.Delay(1000);
                leg2(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 3;
                    await _context.SaveChangesAsync();
                }                
                leg3(flightId);
            }
        }
        private async void leg3(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg3");

            if(_context.flights.Any(flight => flight.LegLocation == 4)) {
                await Task.Delay(1000);
                leg3(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 4;
                    await _context.SaveChangesAsync();
                }                
                leg4(flightId);
            }
        }
        private async void leg4(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg4");

            if(_context.flights.Any(flight => flight.LegLocation == 5)) {
                await Task.Delay(1000);
                leg4(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 5;
                    await _context.SaveChangesAsync();
                }                
                leg5(flightId);
            }
        }
        private async void leg5(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg5");

            var leg6Planes = await _context.flights.Where(flight => flight.LegLocation == 6).ToListAsync();
            var leg7Planes = await _context.flights.Where(flight => flight.LegLocation == 7).ToListAsync();
            if(leg6Planes.Any() || leg7Planes.Any()) {
                await Task.Delay(1000);
                leg5(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    // if(leg6Planes.Count > leg7Planes.Count){
                    //     changeFlightLeg.LegLocation = 7;
                    //     await _context.SaveChangesAsync();
                    //     leg7(flightId);
                    // }
                    // else{
                    //     changeFlightLeg.LegLocation = 6;                    
                    //     await _context.SaveChangesAsync();
                    //     leg6(flightId);
                    // }
                }                
            }
        }
        
    }
}