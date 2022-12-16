using FluentAssertions;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Tracking;
using Tracking.Authentication.ApiKey;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerTests : IClassFixture<WebApplicationFactory<Startup>>//wspoldzielenie factory testy nieco szybsze
    {
        private IConfigurationSection _authSettings;
        private HttpClient _httpClient;

        public TrainerControllerTests(WebApplicationFactory<Startup> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2665
            _authSettings = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build()
           .GetSection("ApiKey");

            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7001/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _httpClient.DefaultRequestHeaders.Add("ApiKey", _authSettings.Value);
        }

        [Fact]
        public async Task Get_TrainerWithApiKeyAuthentication_ReturnStatusOk()
        {
            //arrange

            //act
            var response = await _httpClient.GetAsync("/api/Trainer/1");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_TrainerWithApiKeyAuthentication_ReturnStatusNotFound()
        {
            //arrange

            //act
            var response = await _httpClient.GetAsync("/api/Trainer/100");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }


        [Fact]
        public async Task GetAll_Trainers_WithApiKeyAuthentication_ReturnStatusOk()
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.OK;
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/Trainer");

            //Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

    }
}
