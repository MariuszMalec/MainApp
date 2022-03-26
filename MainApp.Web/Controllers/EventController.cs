﻿using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
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

namespace MainApp.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly ILogger<EventController> _logger;
        private EventService _eventService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        public EventController(ILogger<EventController> logger, EventService eventService, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _eventService = eventService;
            this.httpClientFactory = httpClientFactory;
        }

        // GET: EventController

        [HttpGet]
        [Route("GeAllEventsFromMainApp")]
        public async Task<ActionResult> GeAllEventsFromMainApp()
        {
            _logger.LogInformation("Sciagam dane z bazy z MainApp...");//TODO user is empty???
            var events = await _eventService.GetAll();
            return View(events.OrderByDescending(e => e.CreatedDate).Take(20));
        }

        //--------------------------------------------------------------------------------------
        //sent events to Api
        //--------------------------------------------------------------------------------------
        [HttpGet]
        [Route("SentEvents")]
        public async Task<IActionResult> SentEvents()
        {
            _logger.LogInformation("Sciagam dane z bazy z MainApp...");
            var models = await _eventService.GetAll();
            return Ok(models);
        }

        //--------------------------------------------------------------------------------------
        //get events from api
        //--------------------------------------------------------------------------------------
        [HttpGet]
        public async Task<ActionResult<List<Event>>> Index()
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Tracking");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            return View(events.OrderByDescending(e=>e.CreatedDate).Take(20));
        }

        //TODO jak aktywowac poslanie eventow do api

        //--------------------------------------------------------------------------------------
        //sent events to api tutaj blad 405 gdy post niby niedozowpolona metoda
        //--------------------------------------------------------------------------------------
        // GET: TrainerController/Create
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IEnumerable<Event> events)
        {
            var models = await _eventService.GetAll();

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(models), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);

            return View(models);
        }


    }
}
