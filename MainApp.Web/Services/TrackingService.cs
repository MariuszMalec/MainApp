using MainApp.BLL;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly ILogger<ITrackingService> _logger;
        private readonly IPersonService _userService;
        private readonly IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:7001/api";
        private readonly HttpClient _httpClient;

        public TrackingService(ILogger<ITrackingService> logger, IHttpClientFactory httpClientFactory, IPersonService userService)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
            _userService = userService;
            _httpClient = httpClientFactory.CreateClient("Tracking");//TODO patrz startup
        }

        public async Task<List<Event>> GetAll()
        {
            //HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Tracking");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //przenioslem authoryzacje przez apikey do startupu dzieki dodaniu _httpClient w kosntruktorze
            //request.Headers.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");//TODO Apikey do headera autoryzacji do tracking api

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return new List<Event>();
            }

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            return events;
        }
        public async Task<bool> Insert(Event myEvent)
        {

            //Todo problem z ssl to rozwiazuje jak dodac ogolnie a nie do metody! patrz program.cs
            // HttpClientHandler clientHandler = new HttpClientHandler();
            // clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // HttpClient client = new HttpClient(clientHandler);

            //HttpClient client = httpClientFactory.CreateClient();

            if (myEvent.Id == 0)
            {
                _logger.LogError($"Event can't be id {myEvent.Id}");
                return false;
            }

            var requestUser = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking");

            requestUser.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            requestUser.Content = new StringContent(JsonConvert.SerializeObject(myEvent), Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(requestUser);

            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning($"Insert my event is not authorized! This event was not saved in databae");
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteAllEvents()
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{AppiUrl}/Tracking/DeleteAllEvents");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<Event> GetEventById(int id, string userEmail, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Tracking/{id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("Accept", "application/json");

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<Event>(content);

            return model;
        }

        public async Task<bool> DeleteEvent(int id, Event model, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{AppiUrl}/Tracking/{model.Id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<Event> InsertEvent(ActivityActions activityActions, HttpContext httpContext, string email)
        {
            var userEmail = httpContext.User.Identity.Name;
            if (userEmail == null)
                userEmail = email;
            var user = await _userService.GetByEmail(userEmail);

            var events = GetAll().Result;//TODO musi byc aktywny project tracking!

            //TODO dodanie idy
            var id = 0;
            if (events.Count() == 0)
                id = 1;
            if (events.Count() > 0)
                id = (events?.Max(m => m.Id) ?? 0) + 1;

            return new Event { Id = id, CreatedDate = DateTime.UtcNow, UserId = user.Id, Email = userEmail, Action = activityActions.ToString() };
        }

        public async Task<List<Event>> SelectedEvents(string sortOrder, string searchString, List<Event> events)
        {
            Enum.TryParse<ActivityActions>(searchString, out var selectedAction);
            if (!String.IsNullOrEmpty(selectedAction.ToString()))
            {
                if (selectedAction.ToString() != "All")
                {
                    events = events.Where(s => s.Action.Contains(selectedAction.ToString())).ToList();
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    events = events.OrderByDescending(s => s.Action).ToList();
                    break;
                default:
                    events = events.OrderBy(s => s.Action).ToList();
                    break;
            }
            await Task.Yield();
            return events;
        }
    }
}
