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
        private readonly object contextLock = new object();
        private readonly DataContext _context;
        private Random rnd;
        
        public flightControlService(DataContext context)
        {
            _context = context;
            rnd = new Random();
        }
        public async void addFlight(Flight flight)
        {
            if(canEnterAirport()){
                lock(contextLock)
                {
                    _context.flights.Add(flight);
                    _context.SaveChanges();
                }
                await leg1(flight.Id);
            } else {
                await Task.Delay(1000);
                addFlight(flight);
            }
        }
        public bool canEnterAirport()
        {
            lock(contextLock)
            {
                return !_context.flights.Any(flight => flight.LegLocation == 1);
            }
        }


        private async Task leg1(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg1");
            int count;
            lock(contextLock){
                count = _context.flights.Where(flight => flight.LegLocation == 2).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg1(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 2;
                        _context.SaveChanges();
                    }     
                }
           
                await leg2(flightId);
            }
        }
        private async Task leg2(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg2");
            int count;
            lock(contextLock){
                count = _context.flights.Where(flight => flight.LegLocation == 3).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg2(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 3;
                        _context.SaveChanges();
                    } 
                }               
                await leg3(flightId);
            }
        }

        private async Task leg3(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg3");
            bool found;
            lock(contextLock){
                found = _context.flights.Any(flight => flight.LegLocation == 4);
            }
            if(found) {
                await Task.Delay(1000);
                await leg3(flightId);
            }
            else {
                await Task.Delay(rnd.Next(1000,5000));                
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 4;
                        _context.SaveChanges();
                    }                
                }
                await leg4(flightId, true);
            }
        }
        private async Task leg4(int flightId, bool landing)
        {
            Console.WriteLine("plane "+ flightId + " is on leg4");
            var nextLeg = landing ? 5 : 9;
            int count;
            lock(contextLock){
                count = _context.flights.Where(flight => flight.LegLocation == nextLeg).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg4(flightId, landing);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null) {
                        changeFlightLeg.LegLocation = nextLeg;
                        _context.SaveChanges();
                    }  
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
            List<Flight> leg6Planes;
            List<Flight> leg7Planes;
            lock(contextLock){
                leg6Planes = _context.flights.Where(flight => flight.LegLocation == 6).ToList();
                leg7Planes = _context.flights.Where(flight => flight.LegLocation == 7).ToList();
            }
            if(leg6Planes.Any() || leg7Planes.Any()) {
                await Task.Delay(1000);
                await leg5(flightId);
            }
            else {                    
                await Task.Delay(rnd.Next(1000,5000));

                Flight changeFlightLeg;
                lock(contextLock){
                    changeFlightLeg = _context.flights.Find(flightId);
                }
                if (changeFlightLeg != null)
                {
                    if(leg6Planes.Count > leg7Planes.Count){
                        lock(contextLock){
                            changeFlightLeg.LegLocation = 7;
                            _context.SaveChanges();
                        }
                        await leg7(flightId);
                    }
                    else{
                        lock(contextLock){
                            changeFlightLeg.LegLocation = 6;                    
                            _context.SaveChanges();
                        }
                        await leg6(flightId);
                    }
                }    
                            
            }
        }
        private async Task leg6(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg6");
            int count;
            lock (contextLock){
                count = _context.flights.Where(flight => flight.LegLocation == 8).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg6(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 8;
                        _context.SaveChanges();
                    }  
                }
                await leg8(flightId);
            }
        }
        private async Task leg7(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg7");
            int count;
            lock (contextLock){
                count = _context.flights.Where(flight => flight.LegLocation == 8).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg7(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 8;
                        _context.SaveChanges();
                    }      
                }
                await leg8(flightId);
            }
        }
        private async Task leg8(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg8");
            bool leg4isFree;
            lock(contextLock){
                leg4isFree = _context.flights.Any(flight => flight.LegLocation == 4);
            }
            if(leg4isFree) {
                await Task.Delay(1000);
                await leg8(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                lock(contextLock){
                    var changeFlightLeg = _context.flights.Find(flightId);
                    if (changeFlightLeg != null)
                    {
                        changeFlightLeg.LegLocation = 4;
                        _context.SaveChanges();
                    }     
                }
                await leg4(flightId, false);
            }
        }
        private async Task leg9(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg9");
            await Task.Delay(rnd.Next(1000,5000));
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 0;
                    _context.SaveChanges();
                }                
                var flight = _context.flights.FirstOrDefault(f => f.Id == flightId);
                if (flight != null)
                {
                    _context.flightsLogger.Add(flight);
                    _context.flights.Remove(flight);
                    _context.SaveChanges();
                } 
            }

            // remove from db flights and add to flightslogger
            Console.WriteLine("departed!!!");
        }
        
    }
}