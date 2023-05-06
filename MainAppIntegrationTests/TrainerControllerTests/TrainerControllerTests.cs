using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.IO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Xunit;
using System.Net;

namespace MainAppIntegrationTests.TrainerControllerTests
{
    public class TrainerControllerTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private IConfigurationSection _authSettings;
        private HttpClient _httpClient;

        public TrainerControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Get_TrainerWithApiKeyAuthentication_ReturnStatusNotFound()
        {
            //arrange

            //act
            var response = await _httpClient.GetAsync("/api/Trainer/100");

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Theory]
        [InlineData("/api/Trainer")]
        [InlineData("/api/Trainer/1")]
        public async Task GetAll_EndPoints_WithApiKeyAuthentication_ReturnStatusOk(string endpoint)
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.OK;
            var request = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}");

            //Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
