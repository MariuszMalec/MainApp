using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using MainApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Moq;
using Serilog;
using System.Net;

namespace MainApp.Web.Tests.RoleControllerTests
{
    public class RoleControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> _factory = new WebApplicationFactory<ProgramMVC>();

        public RoleControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Theory]
        [InlineData("/Role")]
        [InlineData("/Role/Details/1")]
        [InlineData("/Role/Edit/1")]//only admin
        public async Task Get_EndPointsReturnsSuccess(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Index_GetCorrectName_ReturnsSuccess()
        {
            //arange
            var roles = new List<ApplicationRoles>()
                {
                    new ApplicationRoles()
                    {
                        Id = 1,
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    }
                };

            var mockRepo = new Mock<IRepositoryService<ApplicationRoles>>();
            mockRepo.Setup(r => r.GetAll())
                .ReturnsAsync(roles);

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            var controller = new RoleController(mockRepo.Object, logger.Object);

            // Act
            var result = await controller.Index();
            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var applicationRoles = Assert.IsAssignableFrom<IEnumerable<ApplicationRoles>>(viewResult.Model);
            // Assert
            Assert.Equal("SuperAdmin", applicationRoles.Select(x => x.Name).FirstOrDefault());
        }

        [Fact]
        public async Task Create_ModelStateValid_ReturnsSuccess()
        {
            //arange
            var role = new ApplicationRoles()
            {
                Id = 3,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            ApplicationRoles? emp = null;
            var mockRepo = new Mock<IRepositoryService<ApplicationRoles>>();
            mockRepo.Setup(r => r.Insert(It.IsAny<ApplicationRoles>()))
                .Callback<ApplicationRoles>(x => emp = x);

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            var controller = new RoleController(mockRepo.Object, logger.Object);

            // Act
            await controller.Create(role);

            // Assert
            mockRepo.Verify(x => x.Insert(It.IsAny<ApplicationRoles>()), Times.Once);
            Assert.Equal(emp.Name, role.Name);
        }

        [Fact]
        public async Task Create_ActionExecuted_RedirectsToIndexAction()
        {
            //arange
            var role = new ApplicationRoles()
            {
                Id = 3,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            ApplicationRoles? emp = null;
            var mockRepo = new Mock<IRepositoryService<ApplicationRoles>>();
            mockRepo.Setup(r => r.Insert(It.IsAny<ApplicationRoles>()))
                .ReturnsAsync(true);

            var logger = new Mock<ILogger>();
            logger.Setup(c => c.Information(It.IsAny<string>()))
                 ;

            var controller = new RoleController(mockRepo.Object, logger.Object);

            // Act
            var result = await controller.Create(role);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
