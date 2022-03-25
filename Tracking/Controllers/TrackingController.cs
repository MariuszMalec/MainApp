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
        [Route("GetAllEvents")]
        public IActionResult Get()
        {   
            List<Event> events = new List<Event>() { new Event() {Action = "Gowno", Email="sdsd", CreatedDate = DateTime.UtcNow, User = null } };

            return Ok(events);
        }





        // GET api/<GetEventsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GetEventsController>
        [HttpGet]
        [Route("Send")]
        public IActionResult Send([FromBody] Event user)
        {
            return Ok(user);
        }

        // PUT api/<GetEventsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GetEventsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
