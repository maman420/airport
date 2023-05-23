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
        private readonly ILogger<mainController> _logger;
        private readonly Repository _repository;
        public mainController(Repository repository, flightControlService flightControlService, IHubContext < airportHub, IairportHub > airportHub, ILogger<mainController> logger)
        {
            _flightControlService = flightControlService;
            _airportHub = airportHub;
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlights()
        {
            IEnumerable<Flight> allFlights = _repository.GetAll().ToList();
            string allFlightsJson = JsonConvert.SerializeObject(allFlights);
            await _airportHub.Clients.All.SendAllFlights(allFlightsJson);
            return Ok(allFlightsJson);
        }
        [HttpGet("flightLogger")]
        public async Task<ActionResult<IEnumerable<Flight>>> GetAllFlightLogger()
        {
            IEnumerable<FlightLogger> allFlights = _repository.GetAllFlightLogger().ToList();
            string allFlightsJson = JsonConvert.SerializeObject(allFlights);
            await _airportHub.Clients.All.SendAllFlights(allFlightsJson);
            return Ok(allFlightsJson);
        }
        [HttpGet("flight")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            // this will be requested from the react app to present it in real time(signal r)
            Flight singleFlight = _repository.FindFlight(id);
            string singleFlightJson = JsonConvert.SerializeObject(singleFlight);
            return Ok(singleFlightJson);
        }
        [HttpPost]
        public IActionResult AddFlightFromAir(Flight flight)
        {
            // this will be requested from the simulator that will add flights,
            // when flight is added, this function will put the flight in leg 1 or tell him to wait in the sky.
            _flightControlService.addFlightFromAir(flight);
            return Ok();
        }
        [HttpPost("fromTerminal")]
        public IActionResult AddFlightFromTerminal(Flight flight)
        {
            // this will be requested from the simulator that will add flights,
            // when flight is added, this function will put the flight in leg 6/7 or tell him to wait in the terminal.
            _flightControlService.addFlightFromTerminal(flight);            
            return Ok();
        }
        [HttpDelete]
        [Route("deleteAll")]
        public async Task<ActionResult> DeleteAll()
        {
            _repository.DeleteAll();
            return Ok();
        }
        [HttpDelete]
        [Route("deleteFlight/{id}")]
        public async Task<ActionResult> DeleteFlightAsync(int id)
        {
            int deletedOrNot = _repository.DeleteFlight(id);
            if(deletedOrNot == 1)
                return Ok();
            else
                return NotFound();
        }
    }
}