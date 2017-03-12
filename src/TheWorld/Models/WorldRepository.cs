using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting trip data from database");
            return _context.Trips.ToList();
        }

        public IEnumerable<Trip> GetAllUserTrips(string username)
        {
            return _context.Trips.Include(t => t.Stops).Where(t => t.UserName == username).ToList();
        }

        public void AddTrip(Trip trip)
        {
            _context.Add(trip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName).FirstOrDefault();
        }

        public Trip GetUserTripByName(string tripName, string username)
        {
            return _context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName && t.UserName == username).FirstOrDefault();
        }

        public void AddStopToTrip(Stop stop, string tripName, string username)
        {
            var trip = GetUserTripByName(tripName, username);
            if(trip!=null)
            {
                _context.Stops.Add(stop);       //todo redundant?
                trip.Stops.Add(stop);
            }
        }

    }
}
