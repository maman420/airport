using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using server.DAL;
using server.Models;
using server.Services;
using server.Hubs;
using Newtonsoft.Json;

namespace server.Controllers
{
    [ApiController]
    [Route("")]
    public class mainController : ControllerBase
    {
        private IHubContext < airportHub, IairportHub > _airportHub;
        private readonly flightControlService _flightControlService;
        private readonly DataContext _context;
        public mainController(flightControlService flightControlService, DataContext context, IHubContext < airportHub, IairportHub > airportHub)
        {
            _flightControlService = flightControlService;
            _context = context;
            _airportHub = airportHub;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            IEnumerable<Flight> allFlights = _context.flights.ToList();
            string allFlightsJson = JsonConvert.SerializeObject(allFlights);
            await _airportHub.Clients.All.SendData(allFlightsJson);

            return Ok(allFlightsJson);
        }

        [HttpGet("flight")]
        public ActionResult<Flight> getFlight(int id)
        {
            // this will be requested from the react app to present it in real time(signal r)
            Flight singleFlight = _context.flights.FirstOrDefault(f => f.Id == id);
            string singleFlightJson = JsonConvert.SerializeObject(singleFlight);
            
            return Ok(singleFlightJson);
        }
        
        [HttpPost]
        public async Task<IActionResult> addFlightFromAir(Flight flight)
        {
            // this will be requested from the simulator that will add flights,
            // when flight is added, this function will put the flight in leg 1 or tell him to wait in the sky.
            await _flightControlService.addFlightFromAir(flight);            
            return Ok();
        }

        [HttpPost("fromTerminal")]
        public async Task<IActionResult> addFlightFromTerminal(Flight flight)
        {
            // this will be requested from the simulator that will add flights,
            // when flight is added, this function will put the flight in leg 6/7 or tell him to wait in the terminal.
            await _flightControlService.addFlightFromTerminal(flight);            
            return Ok();
        }

        [HttpDelete]
        [Route("deleteAll")]
        public ActionResult delete()
        {
            _context.flights.RemoveRange(_context.flights);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        [Route("deleteFlight/{id}")]
        public ActionResult deleteFlight(int id)
        {
            var flight = _context.flights.FirstOrDefault(f => f.Id == id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.flights.Remove(flight);
            _context.SaveChanges();
            return Ok();
        }
    }
}