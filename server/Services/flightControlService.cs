using server.DAL;
using server.Models;

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
        public async Task addFlightFromAir(Flight flight)
        {                
            await Task.Delay(1000);
            if(canEnterAirport()){
                lock(contextLock)
                {
                    _context.flights.Add(flight);
                    _context.SaveChanges();
                }
                await leg1(flight.Id);
            } else {
                await addFlightFromAir(flight);
            }
        }
        public async Task addFlightFromTerminal(Flight flight)
        {
            List<Flight> leg6Planes;
            List<Flight> leg7Planes;
            lock(contextLock){
                leg6Planes = _context.flights.Where(flight => flight.LegLocation == 6).ToList();
                leg7Planes = _context.flights.Where(flight => flight.LegLocation == 7).ToList();
            }
            if(leg6Planes.Any() && leg7Planes.Any()) {
                await Task.Delay(1000);
                await addFlightFromTerminal(flight);
            }
            else {                    
                await Task.Delay(rnd.Next(1000,5000));

                if(leg6Planes.Count > leg7Planes.Count){
                    lock(contextLock){                        
                        flight.LegLocation = 7;
                        _context.flights.Add(flight);
                        _context.SaveChanges();
                    }
                    await leg7(flight.Id);
                }
                else{
                    lock(contextLock){
                        flight.LegLocation = 7;
                        _context.flights.Add(flight);
                        _context.SaveChanges();
                    }
                    await leg6(flight.Id);
                }
            }  
        }
        public bool canEnterAirport()
        {
            lock(contextLock)
            {
                return !_context.flights.Any(flight => flight.LegLocation == 1 || flight.LegLocation == 2 || flight.LegLocation == 3);
            }
        }
        private async Task leg1(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg1");
            int count;
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 1;
                    _context.SaveChanges();
                }                   
                count = _context.flights.Where(flight => flight.LegLocation == 2).ToList().Count();
            }                     
            if(count > 5) {
                await Task.Delay(1000);
                await leg1(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                await leg2(flightId);
            }
        }
        private async Task leg2(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg2");
            int count;
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 2;
                    _context.SaveChanges();
                }                 
                count = _context.flights.Where(flight => flight.LegLocation == 3).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg2(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));            
                await leg3(flightId);
            }
        }
        private async Task leg3(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg3");
            bool is4legFree;
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null)
                {
                    changeFlightLeg.LegLocation = 3;
                    _context.SaveChanges();
                }                
            }                
            await Task.Delay(rnd.Next(1000,5000));                
            lock(contextLock){
                is4legFree = _context.flights.Any(flight => flight.LegLocation == 4);
            }

            if(is4legFree) {
                await Task.Delay(1000);
                await leg3(flightId);
            }
            else {
                await leg4(flightId, true);
            }
        }
        private async Task leg4(int flightId, bool landing)
        {
            Console.WriteLine("plane "+ flightId + " is on leg4");
            var nextLeg = landing ? 5 : 9;
            int count;
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 4;
                    _context.SaveChanges();
                }  
                count = _context.flights.Where(flight => flight.LegLocation == nextLeg).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg4(flightId, landing);
            }
            else {                
                if(landing){
                    await Task.Delay(rnd.Next(1000,3000));
                    await leg5(flightId); 
                }else{
                    await Task.Delay(rnd.Next(3000,5000));
                    await leg9(flightId);
                }
            }
        }
        private async Task leg5(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg5");
            Flight changeFlightLeg;
            List<Flight> leg6Planes;
            List<Flight> leg7Planes;
            lock(contextLock){
                changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 5;
                    _context.SaveChanges();
                }
                leg6Planes = _context.flights.Where(flight => flight.LegLocation == 6).ToList();
                leg7Planes = _context.flights.Where(flight => flight.LegLocation == 7).ToList();
            }
            if(leg6Planes.Any() && leg7Planes.Any()) {
                await Task.Delay(1000);
                await leg5(flightId);
            }
            else {                    
                await Task.Delay(rnd.Next(1000,5000));
                if (changeFlightLeg != null) {
                    if(leg6Planes.Count > leg7Planes.Count){
                        await leg7(flightId);
                    }
                    else{
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
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 6;
                    _context.SaveChanges();
                }  
                count = _context.flights.Where(flight => flight.LegLocation == 8).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg6(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                await leg8(flightId);
            }
        }
        private async Task leg7(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg7");
            int count;
            lock (contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 7;
                    _context.SaveChanges();
                }    
                count = _context.flights.Where(flight => flight.LegLocation == 8).ToList().Count();
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg7(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                await leg8(flightId);
            }
        }
        private async Task leg8(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg8");
            bool leg4isFree;
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 8;
                    _context.SaveChanges();
                }  
                leg4isFree = _context.flights.Any(flight => flight.LegLocation == 4);
            }
            if(leg4isFree) {
                await Task.Delay(1000);
                await leg8(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,5000));
                await leg4(flightId, false);
            }
        }
        private async Task leg9(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg9");
            lock(contextLock){
                var changeFlightLeg = _context.flights.Find(flightId);
                if (changeFlightLeg != null) {
                    changeFlightLeg.LegLocation = 9;
                    _context.SaveChanges();
                }                
            }
            await Task.Delay(rnd.Next(1000,5000));
            departing(flightId);
        }
        private void departing(int flightId)
        {
            lock (contextLock){
                var flight = _context.flights.Find(flightId);
                if (flight != null)
                {
                    flight.LegLocation = 0;
                    _context.SaveChanges();
                }
            }
            Console.WriteLine("departed!!!");
        }
    }
}