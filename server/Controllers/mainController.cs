using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Services;

namespace server.Controllers
{
    [ApiController]
    [Route("")]
    public class mainController : ControllerBase
    {
        private readonly flightControlService _flightControlService;
        public mainController(flightControlService flightControlService)
        {
            _flightControlService = flightControlService;
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
        public ActionResult<IEnumerable<Flight>> getAllFlights()
        {
            // this will be requested from the react app to present it in real time(signal r)
            return Ok();
        }

        [HttpGet("flight")]
        public ActionResult<Flight> getFlight()
        {
            // this will be requested from the react app to present it in real time(signal r)
            return Ok();
        }

    }
}