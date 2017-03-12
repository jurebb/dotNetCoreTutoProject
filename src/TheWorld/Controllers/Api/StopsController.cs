using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;
        private ShamGeoService _service;

        public StopsController(ILogger<StopsController> logger, IWorldRepository repository, ShamGeoService service)
        {
            _logger = logger;
            _repository = repository;
            _service = service;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetUserTripByName(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopsViewModel>>(trip.Stops.OrderBy(s => s.Order).ToList())); 
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error getting stops: {ex}");
            }

            return BadRequest($"Error getting stops");
        }
        
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]StopsViewModel stopVM, string tripName)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var stop = Mapper.Map<Stop>(stopVM);

                    //longitude i latitude
                    var location = await _service.DetermineLocation(stop);
                    if (!location.Result)
                    {
                        _logger.LogError("could not determine long/lat");
                    }
                    else
                    {
                        stop.Latitude = location.Latitude;
                        stop.Longitude = location.Longitude;

                        _repository.AddStopToTrip(stop, tripName, User.Identity.Name);
                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"api/trips/{tripName}/stops/{stop.Name}", Mapper.Map<StopsViewModel>(stop));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error saving stop: {ex}");
            }
            return BadRequest("Error saving stop");
        }
    }
}
