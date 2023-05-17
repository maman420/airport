using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using server.Models;
namespace server.DAL
{
    public interface IRepository
    {
        IEnumerable<Flight> GetAll();
        Flight? FindFlight(int id);
        void DeleteAll();
        int DeleteFlight(int id);
        void AddFlight(Flight flight);
        IEnumerable<Flight> AllFlightsInLeg(int legNum);
        bool isPlanesInLeg(int legNum);
        void ChangeLeg(int flightId, int newLegNum);
        int howMuchInLeg(int legNum);
    }
}
