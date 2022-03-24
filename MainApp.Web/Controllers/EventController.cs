﻿using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private EventService _eventService;
        IHttpClientFactory httpClientFactory;

        public EventController(ILogger<EventController> logger, EventService eventService, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _eventService = eventService;
            this.httpClientFactory = httpClientFactory;
        }

        // GET: EventController
        public async Task<ActionResult> Index()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");//TODO user is empty???
            var models = await _eventService.GetAll();
            return View(models);
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Sciagam dane z bazy danych...");//TODO user is empty???
            var models = await _eventService.GetAll();
            return Ok(models);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Event myEvent)
        {
            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44311/api/Activity");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(myEvent), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var createdAlbum = JsonConvert.DeserializeObject<Event>(content);

            return Ok(content);
        }

        // GET: EventController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EventController/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: EventController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EventController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: EventController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EventController/Delete/5
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var model = await _eventService.GetById(id);
        //    if (model == null)
        //    {
        //        _logger.LogWarning($"Event with Id {id} doesn't exist!");
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(model);
        //}

        //// POST: TrainerController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(int id, Event model)
        //{
        //    try
        //    {
        //        await _eventService.Delete(model);
        //        _logger.LogWarning($"Delete event with id {model.Id}");

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
