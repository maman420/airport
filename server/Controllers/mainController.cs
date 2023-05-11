using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    [EnableCors("AllowAll")]
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
        
        [HttpPost]
        public IActionResult addFlight(Flight flight)
        {
            // this will be requested from the simulator that will add flights,
            // when flight is added, this function will put the flight in leg 1 or tell him to wait in the sky.
            _flightControlService.addFlight(flight);            
            return Ok();
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
        public ActionResult<Flight> getFlight()
        {
            // this will be requested from the react app to present it in real time(signal r)
            return Ok();
        }

        [HttpDelete]
        public ActionResult delete()
        {
            _context.flights.RemoveRange(_context.flights);
            _context.SaveChanges();
            return Ok();
        }

    }
}