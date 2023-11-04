using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class TrainersService : ITrainersService
    {
        private readonly ILogger<TrainersService> _logger;
        private readonly ITrackingService _trackingService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:7001/api";
        private readonly HttpClient _httpClient;

        public TrainersService(IHttpClientFactory httpClientFactory, ILogger<TrainersService> logger, ITrackingService trackingService)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
            _trackingService = trackingService;
            _httpClient = httpClientFactory.CreateClient("Tracking");//TODO patrz startup
        }

        public async Task<List<TrainerView>> GetAll(string userEmail, HttpContext httpContext)//as query authorize
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer/{userEmail}/admin");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await _httpClient.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            if (content.Contains("401") || content.Contains("401"))
            {
                return new List<TrainerView>();
            }

            var trainers = JsonConvert.DeserializeObject<List<TrainerView>>(content);

            if (trainers.Count > 0)
            {
                var myEvent = await _trackingService.InsertEvent(ActivityActions.ViewTrainers, httpContext, userEmail);
                await _trackingService.Insert(myEvent);
            }

            return trainers;
        }
        public async Task<TrainerView> GetTrainerById(int id, string userEmail, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer/{id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("Accept", "application/json");

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<TrainerView>(content);

            var myEvent = await _trackingService.InsertEvent(ActivityActions.detail, httpContext, userEmail);
            await _trackingService.Insert(myEvent);

            return model;
        }

        public async Task<bool> CreateTrainer(TrainerView model, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var userEmail = httpContext.User.Identity.Name;

            var emailTrainers = await GetAll(userEmail, httpContext);
            if (emailTrainers.Any(e => e.Email == model.Email))
            {
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Trainer");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(request);

            var myEvent = await _trackingService.InsertEvent(ActivityActions.create, httpContext, userEmail);
            await _trackingService.Insert(myEvent);

            return true;
        }

        public async Task<bool> EditTrainer(int id, TrainerView model, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var userEmail = httpContext.User.Identity.Name;

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Trainer/{id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(request);

            var myEvent = await _trackingService.InsertEvent(ActivityActions.edit, httpContext, userEmail);
            await _trackingService.Insert(myEvent);

            return true;
        }

        public async Task<bool> DeleteTrainer(int id, TrainerView model, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{AppiUrl}/Trainer/{model.Id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await _httpClient.SendAsync(request);

            if (!result.IsSuccessStatusCode)
            {
                return false;
            }

            var myEvent = await _trackingService.InsertEvent(ActivityActions.delete, httpContext, model.Email);
            await _trackingService.Insert(myEvent);

            return true;
        }

        public async Task<bool> CheckIfEmailExis(string email, HttpContext httpContext)
        {
            var emailTrainers = await GetAll(email, httpContext);
            if (emailTrainers.Any(e => e.Email == email))
            {
                return false;
            }
            return true;
        }
    }
}
