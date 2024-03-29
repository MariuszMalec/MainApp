﻿using FluentAssertions;
using MainApp.BLL;
using MainApp.BLL.Enums;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using Tracking.Models;

namespace MainApp.Web.Tests.TrackingServiceTests
{
    public class TrackingServiceTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private readonly TrackingService _sut;
        private readonly Mock<ILogger<TrackingService>> _loggerMock = new Mock<ILogger<TrackingService>>();
        private readonly Mock<IPersonService> _userService = new Mock<IPersonService>();
        private readonly HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<Tracking.Services.IRepositoryService<Event>> _mockTrackingService = new Mock<Tracking.Services.IRepositoryService<Event>>();
        private readonly IConfiguration _configuration;
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly Mock<HttpContext> _mockHttpContext = new Mock<HttpContext>();

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

            //TODO set configuration
            // 1 way (more lines)
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Provider","UnitTest"},
            };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            _configuration = new ConfigurationBuilder()
                                    .AddInMemoryCollection(inMemorySettings)
                                    .Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
                              // 2 way
            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Provider")]).Returns("UnitTests");

            //TODO zamokowac http i userservice
            _sut = new TrackingService(_loggerMock.Object, _httpClientFactory.Object, _userService.Object, _configurationMock.Object);
        }


        [Fact]
        public async Task DeleteAllEvents_FromMemory_ReturnOk_WhenTrue()
        {
            // Act
            var myEvent = await _sut.DeleteAllEvents();

            //assert
            myEvent.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteEvent_FromMemory_ReturnOk_WhenTrue()
        {
            // Act
            var myEvent = await _sut.DeleteEvent(1, GetEventFromMemory(), _mockHttpContext.Object);

            //assert
            myEvent.Should().BeTrue();
        }

        [Fact]
        public async Task GetEventById_FromMemory_ReturnOk_WhenExist()
        {
            // Act
            var myEvent = await _sut.GetEventById(1, "Admin@example.com", _mockHttpContext.Object);

            //assert
            myEvent.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAll_Events_FromMemory_ReturnOk_WhenExist()
        {
            // Act
            var events = await _sut.GetAll(null, null);

            var expectedNumbersEvents = 7;//look in tracking SeedEvents

            //assert
            events.Should().HaveCount(expectedNumbersEvents);
        }

        [Fact]
        public async Task Insert_Event_ReturnOk_WhenCreate()
        {
            // Act
            var create = await _sut.Insert(GetEvent());

            //assert
            Assert.True(create);
        }

        [Fact]
        public async Task SelectedEvents_ReturnOk_WhenExist()
        {
            // Act
            var selectedEvents = await _sut.SelectedEvents("All", "create", GetEvents());

            //assert
            selectedEvents.Should().HaveCount(1);
            selectedEvents.Any(x => x.Action == "create").Should().BeTrue();
        }

        [Theory]
        [InlineData("Admin@example.com", 2, "login")]
        [InlineData("User@example.com", 1, "login")]
        [InlineData("Admin@example.com", 1, "register")]
        public async Task ActivitySelectedByMail_NumberOfLogin_ReturnOk_WhenIsCorrect(string email, int nmblogin, string activity)
        {
            // Act
            var events = await _sut.ActivitySelectedByMail(email);

            int getValueOfLogin = events[activity]; ;

            //assert
            Assert.Equal(nmblogin, getValueOfLogin);
        }

        private MainApp.BLL.Entities.Event GetEvent()
        {
            var myEvent = new MainApp.BLL.Entities.Event()
            {
                Id = 12,
                Action = ActivityActions.create.ToString(),
                CreatedDate = DateTime.Now,
                UserId = 1,
                Email = "Admin@example.com"
            };
            return myEvent;
        }

        private MainApp.BLL.Entities.Event GetEventFromMemory()
        {
            var myEvent = new MainApp.BLL.Entities.Event()
            {
                Id = 1,
                CreatedDate = DateTime.Now,
                UserId = 1,
                Email = "Admin@example.com",
                Action = "register"
            };
            return myEvent;
        }

        private List<MainApp.BLL.Entities.Event> GetEvents()
        {
            var events = new List<MainApp.BLL.Entities.Event>()
            {
                new MainApp.BLL.Entities.Event { Action = ActivityActions.create.ToString(), CreatedDate = DateTime.Now, UserId = 1, Email = "Admin@example.com" }
            };
            return events;
        }
    }
}
