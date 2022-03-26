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
using Tracking.Models;

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

        public TrackingController(IHttpClientFactory httpClientFactory, ILogger<TrackingController> logger)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
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

        //---------------------------------------------------------
        //wziecie eventow z mainApp
        //---------------------------------------------------------
        [HttpGet]
        [Route("GetEvents")]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {

            _logger.LogInformation("Sciagam dane z bazy danych MainApp...");

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{MainAppUrl}/SentEvents");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            //_logger.LogInformation($"Uzytkownik wyswietlil liste o godz {DateTime.Now}");

            return Ok(events);
        }


        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] IEnumerable<Event> events)
        {
            if (events == null)
                return BadRequest("Brak uzytkownika!");

            //var getEvents = events.ToString();

            //var eventy = JsonConvert.DeserializeObject<IEnumerable<Event>>(getEvents).ToList();

            //foreach (var item in eventy)
            //{
            //    _logger.LogInformation(item.Action);
            //}

            //var content = await result.Content.ReadAsStringAsync();

            //var users = JsonConvert.DeserializeObject<List<Event>>(content);

            //_userService.Insert(user);
            return Ok($"Added events to database");
        }


    }
}
