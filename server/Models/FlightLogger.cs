using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class FlightLogger
    {
        public int Id { get; set; }
        public int Leg { get; set; }
        public Flight Flight { get; set; }
        public DateTime Enter { get; set; }
        public DateTime Exit { get; set; }

    }
}