using FluentAssertions;
using MainApp.BLL.Entities;
using MainApp.BLL.Services;
using MainApp.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace MainAppIntegtratedMvcTests.RoleControllerTests
{
    public class RoleControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();

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

        [Theory]
        [InlineData("/Role/Edit/1")]//only admin
        public async Task Get_Edit_EndPointsReturnsSuccess(string url)
        {
            //arange
            var roles = new ApplicationRoles()
            {
                Id = 1,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(roles), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithAdminClaims();

            //_client = factory.CreateClientWithTestAuth(provider);
            //Act
            var response = await _client.SendAsync(request);

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            // Assert
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
            Assert.Equal("SuperAdmin", applicationRoles.Select(x=>x.Name).FirstOrDefault()); 
        }
    }
}
