using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tracking.Context;
using Tracking.Models;
using Tracking.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        IHttpClientFactory httpClientFactory;
        private readonly ILogger<TrackingController> _logger;
        private const string MainAppUrl = "https://localhost:5001";
        private TrackingService _eventService;
        private readonly MainApplicationContext _context;

        private readonly IRepositoryService<User> _userService;

        public TrackingController(IHttpClientFactory httpClientFactory, ILogger<TrackingController> logger, TrackingService eventService, MainApplicationContext context, IRepositoryService<User> userService)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
            _eventService = eventService;
            _context = context;
            _userService = userService;
        }


        // GET: api/<GetEventsController>
        [HttpGet]
        public IActionResult Get()
        {
            var events = _eventService.GetAll();
            return Ok(events);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Event myEvent)
        {
            if (myEvent == null)
                return BadRequest("Brak eventa!");
            _eventService.Insert(myEvent);
            //return Ok($"User with id {user.Id} added");
            return CreatedAtAction(nameof(Get), new { id = myEvent.Id }, myEvent);
        }


    }
}
