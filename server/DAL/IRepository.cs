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
        Task<Flight?> FindFlight(int id);
        Task DeleteAll();
        Task<int> DeleteFlight(int id);
        Task AddFlight(Flight flight);
        IEnumerable<Flight> AllFlightsInLeg(int legNum);
        bool isPlanesInLeg(int legNum);
        Task ChangeLeg(int flightId, int newLegNum);
        int howMuchInLeg(int legNum);
    }
}
