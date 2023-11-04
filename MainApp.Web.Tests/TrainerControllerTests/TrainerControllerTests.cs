using MainApp.BLL.Models;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using ILogger = Serilog.ILogger;

namespace MainApp.Web.Tests.TrainerControllerTests
{
    public class TrainerControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private Mock<ITrackingService> _mockTracking = new Mock<ITrackingService>();
        private Mock<ILogger<TrainersService>> _loggerMock = new Mock<ILogger<TrainersService>>();
        private Mock<ITrainersService> _mockTrainer = new Mock<ITrainersService>();
        WebApplicationFactory<ProgramMVC> _factory = new WebApplicationFactory<ProgramMVC>();

        public TrainerControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory)
        {
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
        }

        [Fact]
        public async Task Index_GetCorrectName_ReturnsSuccess()
        {
            //arange
            var mockRepo = new Mock<ITrainersService>();
            mockRepo.Setup(r => r.GetAll(It.IsAny<string>(), It.IsAny<HttpContext>())).Returns(GetTrainerViews());

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()));

            var _mockTrainer = new Mock<ITrainersService>();
            _mockTrainer.Setup(r => r.GetAll(It.IsAny<string>(), It.IsAny<HttpContext>())).Returns(GetTrainerViews());

            var controller = new TrainerController(_mockTrainer.Object, logger.Object);

            // Act
            var result = await controller.Index("name_desc");

            var viewResult = Assert.IsType<Microsoft.AspNetCore.Mvc.ViewResult>(result.Result);

            var model = Assert.IsAssignableFrom<IEnumerable<TrainerView>>(viewResult.Model).ToList();

            Assert.NotNull(result);
            Assert.Equal("Trainer@example.com", model.Select(x => x.Email).First());
        }

        public async Task<List<TrainerView>> GetTrainerViews()
        {
            var trainers = new List<TrainerView>()
                {
                    new TrainerView()
                    {
                        Id = 1,
                        Email = "Trainer@example.com"
                    }
                };
            await Task.CompletedTask;
            return trainers.ToList();
        }
    }
}
