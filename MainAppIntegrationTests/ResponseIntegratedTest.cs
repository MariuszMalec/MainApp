﻿using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tracking;
using Xunit;

namespace MainAppIntegrationTests
{
    public class ResponseIntegratedTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly ILogger<TrainersService> _logger;
        private readonly TrackingService _trackingService;
        private readonly HttpClient _client;
        private const string AppiUrl = "https://localhost:7001/api";

        public ResponseIntegratedTest(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task Index_ReturnsTrainersWrongResponse_WhenIsNotStatusOK()
        {
            // Arrange
            var response = await _client.GetAsync($"{AppiUrl}/Trainer");//TODO zmien na enpoint ktorego nie ma w api aby wywalilo test
            //response.EnsureSuccessStatusCode();

            // Act
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Index_TrainersController_ReturnsTrainersWrongResponse_WhenIsNotStatusOK()
        {
            // Arrange
            var trainer = new Trainer
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                FirstName = "Piotr",
                LastName = "Grot",
                Email = "pg@example.com",
                PhoneNumber = "505859599"

            };

            //var mockRepo = new Mock<TrainersService>();
            //mockRepo.Setup(repo => repo.GetAll(trainer.Email, this.HttpContext))
            //    .ReturnsAsync(trainer));
            //;

            var trainerService = new TrainersService((IHttpClientFactory)_client, _logger, _trackingService);
            var controller = new TrainerController(trainerService);

            // Act
            var result = await controller.Index("name_desc");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_Trainer_ReturnWrongResponse_WhenIsNotStatusOK()
        {
            // Arrange
            var trainer = new Trainer
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                FirstName = "Piotr",
                LastName =  "Grot",
                Email = "pg@example.com",
                PhoneNumber = "505859599"
                
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Trainer/Create");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(trainer), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.SendAsync(request);


            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


    }
}
