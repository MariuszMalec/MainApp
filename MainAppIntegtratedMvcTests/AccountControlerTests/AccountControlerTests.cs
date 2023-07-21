using AutoMapper.Execution;
using FluentAssertions;
using MainApp.BLL;
using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using MainApp.BLL.Repositories;
using MainApp.BLL.Services;
using MainApp.Web.Controllers;
using MainApp.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tracking.Services;

namespace MainAppIntegtratedMvcTests.AccountControlerTests
{
    public class AccountControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>, IClassFixture<TestingTrackingWebAppFactory<Program>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly MainApp.Web.Services.TrackingService _trackingService;
        private readonly Mock<IPersonService> _userService = new Mock<IPersonService>();
        private readonly Mock<IConfiguration> _configurationMock = new Mock<IConfiguration>();
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Mock<ILogger<ITrackingService>> _loggerMock = new Mock<ILogger<ITrackingService>>();
        private readonly Mock<IHttpClientFactory> _httpClientFactory = new Mock<IHttpClientFactory>();

        private Mock<FakeUserManager> _mock = new Mock<FakeUserManager>();
        private readonly UserManager<ApplicationUser> _userManager2;

        private HttpClient _client;
        private HttpClient _client2;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();
        WebApplicationFactory<Program> factory2 = new WebApplicationFactory<Program>();

        public AccountControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory, TestingTrackingWebAppFactory<Program> factory2)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            _client2 = factory2.CreateClient();
            _client2.BaseAddress = new Uri("https://localhost:7001/");
            _client2.Timeout = new TimeSpan(0, 0, 30);
            _client2.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _client2.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");

            _httpClientFactory.Setup(x => x.CreateClient("Tracking")).Returns(_client2);

        }

        [Theory]
        [InlineData("/api/Tracking")]
        public async Task GetEvent_ReturnsSuccess_WhenStatusOk(string endpoint)//TODO musi byc uruchomiony project tracking! mvc strzela do api aby zarejstrowac event
        {
            var response = await _client2.GetAsync($"{endpoint}");

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            Assert.Equal("/api/Tracking", requestMessage.RequestUri.LocalPath);
            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnsSuccess_WhenStatusRedirect()//TODO musi byc uruchomiony project tracking! mvc strzela do api aby zarejstrowac event
        {
            //TODO jak sprawdzic ze dobrze zarejstrowalo uzytkownika, event jest i wywala exception?! , jak sprawdzac model view??
            //TODO tylko redirect to sukses reszta to brak rejestracji
            var user = new RegisterView()
            {
                FirstName = "Test1",
                LastName = "Test1",
                PhoneNumber = "555-555-555",
                Password = "123456",
                ConfirmPassword = "123456",
                Email = "tt2@example.com"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Account/Register");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithUserClaims();

            var response = await _client.SendAsync(request);

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            Assert.Equal("/Account/Register", requestMessage.RequestUri.LocalPath);
            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnsNotSuccess_WhenStatusOk()
        {
            var user = new RegisterView()
            {
                FirstName = "Test1",
                LastName = "Test1",
                PhoneNumber = "555-555-555",
                Password = "123456",
                ConfirmPassword = "123456",
                Email = "tt4@example.com"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Account/Register");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithUserClaims();
            //_client = factory.CreateClientWithTestAuth(provider);

            var response = await _client.SendAsync(request);

            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ActionExecuted_RedirectsToLoginAction()
        {
            //arange
            var userReg = new RegisterView()
            {
                FirstName = "Tester",
                LastName = "Test",
                Password = "123456",
                ConfirmPassword = "123456",
                PhoneNumber = "666-666-666",
                Email = "tester@example.com",
            };

            var userApp = new ApplicationUser()
            {
                FirstName = "Testerek",
                LastName = "Testerkowski",
                Email = "Testerkowski@example.com",
                PasswordHash="123456",
                
            };

            ApplicationRoles? emp = null;
            var mockRepo = new Mock<IRepository<ApplicationUser>>();
            mockRepo.Setup(r => r.Insert(It.IsAny<ApplicationUser>()))
                .Returns(Task.CompletedTask);

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<ITrackingService>();

            var loggerSerilog = new Mock<Serilog.ILogger>();
            loggerSerilog.Setup(c => c.Information(It.IsAny<string>()));

            _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Provider")]).Returns("UnitTests");

            //TODO zamokowac http i userservice
            var trackingService = new MainApp.Web.Services.TrackingService(logger, _httpClientFactory.Object, _userService.Object, _configurationMock.Object);

            //
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "FakeDatabase")
                .Options;

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "Test",
                    Id = 1,
                    Email = "test@test.it"
                }
            }.AsQueryable();

            var fakeUserManager = new Mock<FakeUserManager>();

            fakeUserManager.Setup(x => x.Users)
                .Returns(users);

            fakeUserManager.Setup(x => x.DeleteAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);
            fakeUserManager.Setup(x =>
                    x.ChangeEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var signInManager = new Mock<FakeSignInManager>();
            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<bool>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new AccountController(fakeUserManager.Object, signInManager.Object, loggerSerilog.Object, _applicationDbContext, mockRepo.Object, trackingService);

            // Act
            var result = await controller.Register(userReg);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectToActionResult.ActionName);
        }

        private IEnumerable<ApplicationUser> GetUsersDto()
        {
            var sessions = new List<ApplicationUser>();
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2016, 7, 2),
                Id = 1,
                FirstName = "Testerek",
                LastName = "Testerkowski",
                Email = "Testerkowski@example.com",
                PasswordHash = "123456",
            });
            sessions.Add(new ApplicationUser()
            {
                Created = new DateTime(2016, 7, 2),
                Id = 2,
                FirstName = "Testerek",
                LastName = "Testerkowski",
                Email = "tester@example.com",
                PasswordHash = "123456",
            });
            return sessions;
        }

    }
}
