using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class TrackingService
    {
        private readonly ILogger<TrackingService> _logger;
        private readonly UserService _userService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:7001/api";

        public TrackingService(ILogger<TrackingService> logger, IHttpClientFactory httpClientFactory, UserService userService)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
            _userService = userService;
        }

        public async Task<List<Event>> GetAll()
        {
            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Tracking");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return new List<Event> ();
            }

            var content = await result.Content.ReadAsStringAsync();

            var events = JsonConvert.DeserializeObject<List<Event>>(content);

            return events;
        }
        public async Task Insert(Event myEvent)
        {
            HttpClient client = httpClientFactory.CreateClient();

            var requestUser = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking");

            requestUser.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            requestUser.Content = new StringContent(JsonConvert.SerializeObject(myEvent), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(requestUser);
        }

        public async Task<bool> DeleteAllEvents()
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete  , $"{AppiUrl}/Tracking/DeleteAllEvents");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

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

            var result = await client.SendAsync(request);

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

            var result = await client.SendAsync(request);

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
            return new Event { CreatedDate = DateTime.Now, UserId = user.Id, Email = userEmail, Action = activityActions.ToString()};
        }


    }


}
