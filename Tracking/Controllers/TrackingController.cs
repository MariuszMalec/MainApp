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
        private EventService _eventService;
        private readonly MainApplicationContext _context;

        public TrackingController(IHttpClientFactory httpClientFactory, ILogger<TrackingController> logger, EventService eventService, MainApplicationContext context)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
            _eventService = eventService;
            _context = context;
        }

        //------------------------------------------------------------------------------------------------------------
        //mainApp pobiera te ponizsze dane
        //------------------------------------------------------------------------------------------------------------
        // GET: api/<GetEventsController>
        [HttpGet]
        public IActionResult Get()
        {
            //List<Event> events = new List<Event>() { new Event() {Action = "Gowno", Email="sdsd", CreatedDate = DateTime.UtcNow, User = null } };
            var events = _eventService.GetAll();
            return Ok(events);
        }

        //---------------------------------------------------------
        //wziecie eventow z mainApp
        //---------------------------------------------------------
        [HttpGet]
        [Route("GetEvents")]
        public async Task<ActionResult<List<Event>>> GetEvents()
        {

            //TODO wyczyscic baze events z APi
            _context.Events.RemoveRange(_context.Events);
            _context.SaveChanges();

            _logger.LogInformation("Sciagam dane z bazy danych MainApp...");

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{MainAppUrl}/SentEvents");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            //Zapis do bazy
            foreach (var item in events)
            {
                _eventService.Insert(item);
            }

            return Ok(events);
        }


        [HttpGet]
        [Route("ActiveEvents")]
        public async Task<IActionResult> ActiveEvents()
        {
            await GetEvents();
            return Ok($"Sent events to view");
        }

    }
}
