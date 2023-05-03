using MainApp.BLL;
using MainApp.BLL.Enums;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;

namespace MainAppIntegtratedMvcTests.EventControllerTests
{
    public class EventControllerTests : IClassFixture<WebApplicationFactory<ProgramMVC>>//wspoldzielenie factory testy nieco szybsze
    {
        private readonly ITrackingService _sut;
        private readonly Mock<ITrackingService> _trackingServiceMock = new Mock<ITrackingService>();
        private readonly HttpClient _httpClient;
        private readonly Mock<ILogger<ITrackingService>> _loggerMock = new Mock<ILogger<ITrackingService>>();
        private readonly Mock<ILogger<EventController>> _loggerEventControllerMock = new Mock<ILogger<EventController>>();
        private readonly Mock<IPersonService> _userService = new Mock<IPersonService>();
        private readonly HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        //private readonly Mock<Tracking.Services.IRepositoryService<Event>> _mockTrackingService = new Mock<Tracking.Services.IRepositoryService<Event>>();

        public EventControllerTests(WebApplicationFactory<ProgramMVC> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
                })
                .CreateClient();

            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");

            _httpClientFactory.Setup(x => x.CreateClient("Tracking")).Returns(_client);//TODO dzieki temu test dziala!

            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Provider")]).Returns("UnitTests");

            //TODO zamokowac http i userservice
            _sut = new TrackingService(_loggerMock.Object, _httpClientFactory.Object, _userService.Object, _configurationMock.Object);

        }

        [Fact]
        public async Task GetEvents_ReturnNotNull_WhenExist()
        {
            // Arrange
            var mockRepo = new Mock<ITrackingService>();
            mockRepo.Setup(r => r.GetAll(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(GetEvents());

            var controller = new EventController(_loggerEventControllerMock.Object, mockRepo.Object);

            // Act
            var actionResult = await controller.Index(string.Empty, string.Empty);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(actionResult);
            mockRepo.Verify(u => u.GetAll(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            Assert.NotNull(viewResult);//TODO how to check getting list??
        }

        private async Task<List<MainApp.BLL.Entities.Event>> GetEvents()
        {
            var events = new List<MainApp.BLL.Entities.Event>()
            {
                new MainApp.BLL.Entities.Event { Id=3, Action = ActivityActions.create.ToString(), CreatedDate = DateTime.Now, UserId = 1, Email = "Admin@example.com" }
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
