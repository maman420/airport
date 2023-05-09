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
        private Random rnd;
        
        public flightControlService(DataContext context)
        {
            _context = context;
            rnd = new Random();

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


        private async Task leg1(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg1");
            if(_context.flights.Where(flight => flight.LegLocation == 2).ToList().Count() > 5) {
                await Task.Delay(1000);
                await leg1(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 2;
                    await _context.SaveChangesAsync();
                }                
                await leg2(flightId);
            }
        }
        private async Task leg2(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg2");

            if(_context.flights.Where(flight => flight.LegLocation == 3).ToList().Count() > 5) {
                await Task.Delay(1000);
                await leg2(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 3;
                    await _context.SaveChangesAsync();
                }                
                await leg3(flightId);
            }
        }
        private async Task leg3(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg3");

            if(_context.flights.Any(flight => flight.LegLocation == 4)) {
                await Task.Delay(1000);
                await leg3(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 4;
                    await _context.SaveChangesAsync();
                }                
                await leg4(flightId, true);
            }
        }
        private async Task leg4(int flightId, bool landing)
        {
            Console.WriteLine("plane "+ flightId + " is on leg4");
            var nextLeg = landing ? 5 : 9;

            if(_context.flights.Where(flight => flight.LegLocation == nextLeg).ToList().Count() > 5) {
                await Task.Delay(1000);
                await leg4(flightId, landing);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = nextLeg;
                    await _context.SaveChangesAsync();
                }  
                if(landing){
                   await leg5(flightId); 
                }else{
                    await leg9(flightId);
                }
            }
        }
        private async Task leg5(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg5");

            var leg6Planes = await _context.flights.Where(flight => flight.LegLocation == 6).ToListAsync();
            var leg7Planes = await _context.flights.Where(flight => flight.LegLocation == 7).ToListAsync();
            if(leg6Planes.Any() || leg7Planes.Any()) {
                await Task.Delay(1000);
                await leg5(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                if (changeFlightLeg != null)
                {
                    await Task.Delay(rnd.Next(1000,5000));
                    if(leg6Planes.Count > leg7Planes.Count){
                        changeFlightLeg.LegLocation = 7;
                        await _context.SaveChangesAsync();
                        await leg7(flightId);
                    }
                    else{
                        changeFlightLeg.LegLocation = 6;                    
                        await _context.SaveChangesAsync();
                        await leg6(flightId);
                    }
                }                
            }
        }
        private async Task leg6(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg6");

            if(_context.flights.Where(flight => flight.LegLocation == 8).ToList().Count() > 5) {
                await Task.Delay(1000);
                await leg6(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 8;
                    await _context.SaveChangesAsync();
                }                
                await leg8(flightId);
            }
        }
        private async Task leg7(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg7");

            if(_context.flights.Where(flight => flight.LegLocation == 8).ToList().Count() > 5) {
                await Task.Delay(1000);
                await leg7(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 8;
                    await _context.SaveChangesAsync();
                }                
                await leg8(flightId);
            }
        }
        private async Task leg8(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg8");

            if(_context.flights.Any(flight => flight.LegLocation == 4)) {
                await Task.Delay(1000);
                await leg8(flightId);
            }
            else {
                var changeFlightLeg = await _context.flights.FindAsync(flightId);
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 4;
                    await _context.SaveChangesAsync();
                }                
                await leg4(flightId, false);
            }
        }
        private async Task leg9(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg9");

            var changeFlightLeg = await _context.flights.FindAsync(flightId);
            await Task.Delay(rnd.Next(1000,5000));
            if (changeFlightLeg != null)
            {
                changeFlightLeg.LegLocation = 0;
                await _context.SaveChangesAsync();
            }                
            var flight = _context.flights.FirstOrDefault(f => f.Id == flightId);
            if (flight != null)
            {
                _context.flightsLogger.Add(flight);
                _context.flights.Remove(flight);
                await _context.SaveChangesAsync();
            }

            // remove from db flights and add to flightslogger
            Console.WriteLine("departed!!!");
        }
        
    }
}