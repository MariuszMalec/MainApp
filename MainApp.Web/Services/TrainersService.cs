using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Models;
using MainApp.BLL.Repositories;
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
    public class TrainersService
    {
        private readonly ILogger<TrainersService> _logger;
        private EventService _eventService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        public TrainersService(IHttpClientFactory httpClientFactory, ILogger<TrainersService> logger, EventService eventService)
        {
            this.httpClientFactory = httpClientFactory;
            _logger = logger;
            _eventService = eventService;
        }

        public async Task<List<TrainerView>> GetAll(string userEmail, HttpContext httpContext)
        {
            _logger.LogInformation("Sciagam dane z bazy danych API...");

            await _eventService.InsertEvent(ActivityActions.ViewTrainers, httpContext, userEmail);

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var trainers = JsonConvert.DeserializeObject<List<TrainerView>>(content);
            return trainers;
        }
        public async Task<TrainerView> GetTrainerById(int id, string userEmail, HttpContext httpContext)
        {
            _logger.LogInformation($"User {userEmail} sprawdza dane uzytkowniaka o id {id}");

            await _eventService.InsertEvent(ActivityActions.detail, httpContext, userEmail);

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer/{id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("Accept", "application/json");

            var result = await client.SendAsync(request);

            var content = await result.Content.ReadAsStringAsync();

            var model = JsonConvert.DeserializeObject<TrainerView>(content);
            return model;
        }

        public async Task<bool> CreateTrainer(TrainerView model, HttpContext httpContext)
        {
            _logger.LogInformation($"Create new trainer with id {model.Id} at {DateTime.Now}");

            HttpClient client = httpClientFactory.CreateClient();

            var userEmail = httpContext.User.Identity.Name;

            await _eventService.InsertEvent(ActivityActions.create, httpContext, model.Email);

            if (userEmail == (string)model.Email)//TODO wziecie maily z bazy i sprawdzenie wszystkich!
            {
                _logger.LogWarning($"Trainer can't be created, email exist yet!");//TODO wyswietlenie komunikatu
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Trainer");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);
            return true;
        }

        public async Task<bool> EditTrainer(int id, TrainerView model, HttpContext httpContext)
        {
            _logger.LogInformation($"Edit trainer with id {model.Id} at {DateTime.Now}");

            HttpClient client = httpClientFactory.CreateClient();

            string userEmail = await _eventService.InsertEvent(ActivityActions.edit, httpContext, model.Email);

            if (userEmail == (string)model.Email)//TODO wziecie maily z bazy i sprawdzenie wszystkich!
            {
                _logger.LogWarning($"Trainer can't be edit, email exist yet!");
                return false;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Trainer/{id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);
            return false;
        }

        public async Task<bool> DeleteTrainer(int id, TrainerView model, HttpContext httpContext)
        {

            HttpClient client = httpClientFactory.CreateClient();

            _logger.LogWarning($"Delete trainer with id {model.Id}");

            await _eventService.InsertEvent(ActivityActions.delete, httpContext, model.Email);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{AppiUrl}/Trainer/{model.Id}");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);   

            return true;
        }
    }
}
