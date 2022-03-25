using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MainApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        IHttpClientFactory httpClientFactory;
        private EventService _eventService;
        private const string AppiUrl = "https://localhost:44311/api";

        public EventsController(IHttpClientFactory httpClientFactory, EventService eventService)
        {
            this.httpClientFactory = httpClientFactory;
            _eventService = eventService;
        }

        // GET: api/<EventsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<EventsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        [HttpPost]
        [Route("Send")]
        public async Task<ActionResult> Send([FromBody] List<Event> myEvent)
        {
            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking/Send");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var models = await _eventService.GetAll();


            request.Content = new StringContent(JsonConvert.SerializeObject(models), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<Event>(content);

            return Ok(result);
        }



        // POST api/<EventsController>
        //[HttpPost]
        //[Route("Send")]
        //public async Task<ActionResult> Send([FromBody] IEnumerable<Event> myEvent)
        //{
        //    HttpClient client = httpClientFactory.CreateClient();

        //    var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking/Send");

        //    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        //    var models = await _eventService.GetAll();


        //    request.Content = new StringContent(JsonConvert.SerializeObject(models), Encoding.UTF8, "application/json");

        //    var result = await client.SendAsync(request);

        //    var content = await result.Content.ReadAsStringAsync();

        //    var events = JsonConvert.DeserializeObject<Event>(content);

        //    return Ok(result);
        //}

        // PUT api/<EventsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EventsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
