using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tracking;
using Xunit;

namespace MainAppIntegrationTests
{
    public class IntegrationTrainerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpClient;
        private readonly IConfigurationSection _authSettings;

        public IntegrationTrainerTests(WebApplicationFactory<Startup> factory)
        {
            _httpClient = factory.CreateClient();

            _authSettings = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build()
               .GetSection("ApiKey");
        }

        [Fact]
        public async Task GetAll_Trainers_WithAuthenticateApiKey_StatusOk()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Trainer");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Headers.Add("ApiKey", _authSettings.Value);//TODO Apikey do headera

            // Act
            var result = await _httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task GetAll_Trainers_WithOutAuthenticateApiKey_StausUnauthorized()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/Trainer");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var result = await _httpClient.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }

}

