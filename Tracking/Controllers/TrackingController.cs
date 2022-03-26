using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tracking.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        IHttpClientFactory httpClientFactory;
        private const string MainAppUrl = "https://localhost:5001";

        public TrackingController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        //------------------------------------------------------------------------------------------------------------
        //mainApp pobiera te ponizsze dane
        //------------------------------------------------------------------------------------------------------------
        // GET: api/<GetEventsController>
        [HttpGet]
        public IActionResult Get()
        {   
            List<Event> events = new List<Event>() { new Event() {Action = "Gowno", Email="sdsd", CreatedDate = DateTime.UtcNow, User = null } };

            return Ok(events);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] IEnumerable<Event> events)
        {
            if (events == null)
                return BadRequest("Brak uzytkownika!");
            //_userService.Insert(user);
            return Ok($"Added events to database");
        }


    }
}
