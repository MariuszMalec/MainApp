using Castle.Components.DictionaryAdapter.Xml;
using MainApp.BLL.Models;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Moq;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MainApp.Web.Tests.TrainerControllerTests
{
    public class TrainerControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();
        private Mock<TrackingService> _mockTracking = new Mock<TrackingService>();
        private Mock<ILogger<TrainersService>> _loggerMock = new Mock<ILogger<TrainersService>>();

        WebApplicationFactory<ProgramMVC> _factory = new WebApplicationFactory<ProgramMVC>();

        public TrainerControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory)
        {
            //_client = factory.CreateClient();
            //_client.BaseAddress = new Uri("https://localhost:5001/");
            //_client.Timeout = new TimeSpan(0, 0, 30);
            //_client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

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

            _httpClientFactory.Setup(x => x.CreateClient("Trainer")).Returns(_client);//TODO dzieki temu test dziala!


        }

        [Fact]
        public async Task Index_GetCorrectName_ReturnsSuccess()
        {
            //arange
            var trainers = new List<TrainerView>()
                {
                    new TrainerView()
                    {
                        Id = 1,
                    }
                }.ToList();

            //var urlReferer = $"https://localhost:5001/Trainer";
            //var httpcontextMock = new Mock<HttpContext>();
            //var requestMock = new Mock<HttpRequest>();
            //var headers = new Mock<IHeaderDictionary>();
            //headers.Setup(x => x["Referer"]).Returns(urlReferer);
            //requestMock.Setup(x => x.Headers).Returns(headers.Object);
            //httpcontextMock.Setup(x=>x.Request).Returns(requestMock.Object);

            


            //httpcontextMock.Setup(h => h.User.Identity.Name).Returns(It.IsAny<string>());

            var mockRepo = new Mock<ITrainersService>();
            mockRepo.Setup(r => r.GetAll(It.IsAny<string>(), It.IsAny<HttpContext>())).ReturnsAsync(GetTrainerViews());

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            //_loggerMock.Setup(c => c.LogInformation(It.IsAny<string>()));

            var trainerServices = new TrainersService(_httpClientFactory.Object, _loggerMock.Object, _mockTracking.Object);

            //tu skonczylem!!!!!!!!!
            var controller = new TrainerController(trainerServices, logger.Object);

            // Act
            var result = await controller.Index("name_desc");
            var viewResult = Assert.IsAssignableFrom<TrainerView>(result);
            //var applicationRoles = Assert.IsAssignableFrom<IEnumerable<ApplicationRoles>>(viewResult.Model);
            // Assert
            Assert.NotNull(viewResult);
        }

        public List<TrainerView>GetTrainerViews()
        {
            return new List<TrainerView>()
                {
                    new TrainerView()
                    {
                        Id = 1,
                    }
                }.ToList();
        }


    }
}
