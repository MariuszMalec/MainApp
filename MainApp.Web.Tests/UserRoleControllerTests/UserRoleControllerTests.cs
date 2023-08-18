using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace MainApp.Web.Tests.UserRoleControllerTests
{
    public class UserRoleControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();

        public UserRoleControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Theory]
        [InlineData("/UserRole")]
        public async Task Get_Index_EndPointsReturnsSuccess(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/UserRole/Details/1")]
        public async Task Get_Details_EndPointsReturnsSuccess(string url)
        {
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/UserRole/Edit/1")]//TODO only admin, brak seeda userrole dlatego not found, do poprawy
        public async Task Get_Edit_EndPointsReturnsNotFound(string url)
        {
            var response = await _client.GetAsync(url);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
