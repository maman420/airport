using Microsoft.AspNetCore.SignalR;
using server.DAL;
using server.Hubs;
using server.Models;

namespace server.Services
{
    public class flightControlService
    {
        private readonly object contextLock = new object();
        private readonly object leg4Lock = new object();
        private readonly Repository _repository;
        private Random rnd;
        private IHubContext < airportHub, IairportHub > _airportHub;
        public flightControlService(Repository repository, IHubContext < airportHub, IairportHub > airportHub)
        {
            rnd = new Random();
            _airportHub = airportHub;
            _repository = repository;
        }
        public async Task addFlightFromAir(Flight flight)
        {                
            await Task.Delay(1000);
            if(canEnterAirport()){
                lock(contextLock)
                {
                    _repository.AddFlight(flight);
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
                leg6Planes = _repository.AllFlightsInLeg(6).ToList();
                leg7Planes = _repository.AllFlightsInLeg(7).ToList();
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
                        _repository.AddFlight(flight);
                    }
                    await leg7(flight.Id);
                }
                else{
                    lock(contextLock){
                        flight.LegLocation = 7;
                        _repository.AddFlight(flight);
                    }
                    await leg6(flight.Id);
                }
            }  
        }
        public bool canEnterAirport()
        {
            lock(contextLock)
            {
                return !_repository.isPlanesInLeg(1);
            }
        }
        private async Task leg1(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg1");
            int count;
            lock(contextLock){
                _repository.ChangeLeg(flightId, 1);
                count = _repository.howMuchInLeg(2);
            }                     
            if(count > 5) {
                await Task.Delay(1000);
                await leg1(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,3000));
                await leg2(flightId);
            }
        }
        private async Task leg2(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg2");
            int count;
            lock(contextLock){
                _repository.ChangeLeg(flightId, 2);            
                count = _repository.howMuchInLeg(3);
            }
            if(count > 5) {
                await Task.Delay(1000);
                await leg2(flightId);
            }
            else {                
                await Task.Delay(rnd.Next(1000,3000));            
                await leg3(flightId);
            }
        }
        private async Task leg3(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg3");
            bool is4legFree;
            lock(contextLock){
                _repository.ChangeLeg(flightId, 3);      
            }                
            await Task.Delay(rnd.Next(1000,3000));                
            lock(leg4Lock){
                lock(contextLock){
                    is4legFree = !_repository.isPlanesInLeg(4);
                }
                if(!is4legFree) {
                    leg3(flightId);
                }
                else {
                    leg4(flightId, true);
                }
            }
        }

        private async Task leg4(int flightId, bool landing)
        {
            Console.WriteLine("plane "+ flightId + " is on leg4");
            var nextLeg = landing ? 5 : 9;
            int count;
            lock(contextLock){
                _repository.ChangeLeg(flightId, 4);
                count = _repository.howMuchInLeg(nextLeg);
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
            List<Flight> leg6Planes;
            List<Flight> leg7Planes;
            lock(contextLock){
                _repository.ChangeLeg(flightId, 5);
                leg6Planes = _repository.AllFlightsInLeg(6).ToList();
                leg7Planes = _repository.AllFlightsInLeg(7).ToList();
            }
            if(leg6Planes.Any() && leg7Planes.Any()) {
                await Task.Delay(1000);
                await leg5(flightId);
            }
            else {                    
                await Task.Delay(rnd.Next(1000,5000));
                if(leg6Planes.Count > leg7Planes.Count){
                    await leg7(flightId);
                }
                else{
                    await leg6(flightId);
                }
            }
        }
        private async Task leg6(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg6");
            int count;
            lock (contextLock){
                _repository.ChangeLeg(flightId, 6);
                count = _repository.howMuchInLeg(8);
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
                _repository.ChangeLeg(flightId, 7);
                count = _repository.howMuchInLeg(8);
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
                _repository.ChangeLeg(flightId, 8);
            }                
            await Task.Delay(rnd.Next(1000,3000));

            lock(leg4Lock){
                lock(contextLock){
                    leg4isFree = !_repository.isPlanesInLeg(4);
                }
                if(!leg4isFree) {
                    leg8(flightId);
                }
                else {                
                    leg4(flightId, false);
                }
            }

        }
        private async Task leg9(int flightId)
        {
            Console.WriteLine("plane "+ flightId + " is on leg9");
            lock(contextLock){
                _repository.ChangeLeg(flightId, 9);
            }
            await Task.Delay(rnd.Next(1000,5000));
            departing(flightId);
        }
        private void departing(int flightId)
        {
            lock (contextLock){
                _repository.ChangeLeg(flightId, 0);
            }
            Console.WriteLine("departed!!!");
        }
    }
}