using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tracking.Authentication.ApiKey;
using Tracking.Context;
using Tracking.Models;
using Tracking.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    //[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.AuthenticationScheme)]
    public class TrackingController : ControllerBase
    {
        private readonly IRepositoryService<Event> _trackingService;
        private readonly MainApplicationContext _context;

        public TrackingController(IRepositoryService<Event> trackingService, MainApplicationContext context)
        {
            _trackingService = trackingService;
            _context = context;
        }

        // GET: api/<GetEventsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var events = await _trackingService.GetAll();
            if (!events.Any())
            {
                return NotFound("List of events is empty!");
            }
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var myEvent = await _trackingService.Get(id);
            if (myEvent == null)
                return BadRequest($"Brak eventa!");
            return Ok(myEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] Event myEvent)
        {
            if (myEvent == null)
                return BadRequest("Brak eventa!");
            await _trackingService.Insert(myEvent);
            //return Ok($"User with id {user.Id} added");
            return CreatedAtAction(nameof(Get), new { id = myEvent.Id }, myEvent);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var myEvent = await _trackingService.Get(id);
            if (myEvent == null)
                return BadRequest($"Brak uzytkownika!");
            await _trackingService.Delete(id);
            return Ok($"User with id {id} deleted");
        }


        [HttpDelete("DeleteAllEvents")]
        public async Task<IActionResult> DeleteAllEvents()
        {
            if (!_context.Events.Any())
                return BadRequest("Brak eventow!");
            _context.Events.RemoveRange(_context.Events);
            await _context.SaveChangesAsync();
            return Ok($"All events were deleted!");
        }


    }
}
