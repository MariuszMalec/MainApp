using FluentAssertions;
using MainApp.BLL;
using MainApp.BLL.Enums;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking.Models;
using Xunit;

namespace MainAppIntegrationTests.TrackingServiceTests
{
    public class TrackingServiceTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private readonly TrackingService _sut;
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<TrackingService>> _loggerMock = new Mock<ILogger<TrackingService>>();
        private readonly Mock<IPersonService> _userService = new Mock<IPersonService>();
        private readonly HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<Tracking.Services.IRepositoryService<Event>> _mockTrackingService = new Mock<Tracking.Services.IRepositoryService<Event>>();

        public TrackingServiceTests(WebApplicationFactory<Program> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
                })
                .CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");

            _httpClientFactory.Setup(x => x.CreateClient("Tracking")).Returns(_client);//TODO dzieki temu test dziala!

            //TODO zamokowac http i userservice
            _sut = new TrackingService(_loggerMock.Object, _httpClientFactory.Object, _userService.Object);

        }

        [Theory]
        [InlineData("/api/Tracking")]
        public async Task GetAll_Events_ReturnOk_WhenExist(string endpoint)
        {
            // Arrange
            _mockTrackingService.Setup(x => x.GetAll()).ReturnsAsync(GetEvents());

            // Act
            var events = await _sut.GetAll();

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/Tracking/Insert")]
        public async Task Insert_Event_ReturnOk_WhenCreate(string endpoint)
        {
            // Arrange
            //_mockTrackingService.Setup(x => x.Insert()).ReturnsAsync(GetEvent());

            // Act
            var create = await _sut.Insert(GetEvent());

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        private IEnumerable<Event> GetEvents()
        {
            var events = new List<Event>()
            {
                new Tracking.Models.Event { Action = ActivityActions.create.ToString(), CreatedDate = DateTime.Now, UserId = 1, Email = "Admin@example.com" }
            };
            return events;
        }

        private MainApp.BLL.Entities.Event GetEvent()
        {
            var myEvent = new MainApp.BLL.Entities.Event()
            {
                Id = 2,
                Action = ActivityActions.create.ToString(),
                CreatedDate = DateTime.Now,
                UserId = 1, 
                Email = "Admin@example.com"
            };
            return myEvent;
        }
    }
}
