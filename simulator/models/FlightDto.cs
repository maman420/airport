using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simulator.models
{
    public class FlightDto
    {
        public string Name { get; set; }
        public int LegLocation { get; set; }
        public string AirLine { get; set; }
        private static readonly string[] _airlines = { "Elal", "American Airlines", "Qatar Airways", "Emirates", "Air France", "Turkish Airlines", "Israir", "Arkia", "Singapore Airlines", "Japan Airlines", "United Airlines"};
        public FlightDto()
        {
            Name = GenerateRandomName(10);
            LegLocation = 0;
            AirLine = GenerateRandomAirline();
        }
        string GenerateRandomAirline()
        {
            var random = new Random();
            return _airlines[random.Next(0,_airlines.Count())];
        }
        string GenerateRandomName(int length)
        {
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = allowedChars[random.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}