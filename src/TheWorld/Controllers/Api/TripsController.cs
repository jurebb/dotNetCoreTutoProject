using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var data = _repository.GetAllUserTrips(this.User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripsViewModel>>(data));
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get all trips: {ex}");
                return BadRequest("Error occured");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripsViewModel recievedTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(recievedTrip);
                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if(await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{recievedTrip.Name}", Mapper.Map<TripsViewModel>(newTrip));
                }          
            }
            return BadRequest("Failed to save the trip");
        }
    }
}
