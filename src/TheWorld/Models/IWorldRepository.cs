using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllUserTrips(string username);

        void AddTrip(Trip trip);

        Task<bool> SaveChangesAsync();

        Trip GetTripByName(string tripName);
        Trip GetUserTripByName(string tripName, string username);

        void AddStopToTrip(Stop stop, string tripName, string username);
    }
}