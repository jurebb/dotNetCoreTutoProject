using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;

namespace TheWorld.Services
{
    public class ShamGeoService
    {
        private ILogger<ShamGeoService> _logger;

        public ShamGeoService(ILogger<ShamGeoService> logger)
        {
            _logger = logger;
        }

        public async Task<ShamGeoResults> DetermineLocation(Stop stop)
        {
            var result = new ShamGeoResults()
            {
                Result = false,
                Message = "could not be found",
            };
            result = await SetLatLong(result);

            return result;
        }

        public async Task<ShamGeoResults> SetLatLong(ShamGeoResults result)
        {
            result.Result = true;
            result.Message = "succesfuly obtained location";
            result.Longitude = 44.444444;
            result.Latitude = 55.555555;
            return result;
        }
    }
}
