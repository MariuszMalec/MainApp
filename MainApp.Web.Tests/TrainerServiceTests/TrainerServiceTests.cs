using FluentAssertions;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;

namespace MainApp.Web.Tests.TrainerServiceTests
{
    public class TrainerServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITrainersService _sut;
        private readonly HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private readonly Mock<ILogger<TrainersService>> _loggerMock = new Mock<ILogger<TrainersService>>();
        private readonly Mock<ITrackingService> _trackingService = new Mock<ITrackingService>();
        public TrainerServiceTests(WebApplicationFactory<Program> factory)
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

            //TODO zamokowac http i userservice
            _sut = new TrainersService(_httpClientFactory.Object, _loggerMock.Object, _trackingService.Object);
        }

        [Fact]
        public async Task GetAll_Trainers_FromMemory_ReturnOk_WhenExist()
        {
            // Act
            var result = await _sut.GetAll("Admin@example.com", null);

            var expectedNumbersEvents = 1;//look in tracking SeedEvents

            //assert
            result.Should().HaveCount(expectedNumbersEvents);
        }

        [Theory]
        [InlineData("Trainer@example.com")]
        public async Task Get_Trainer_FromMemory_ReturnOk_WhenExist(string mail)
        {
            // Act
            var result = await _sut.GetAll("Admin@example.com", null);

            //assert
            Assert.Equal(mail,result.FirstOrDefault().Email);
        }
    }
}
