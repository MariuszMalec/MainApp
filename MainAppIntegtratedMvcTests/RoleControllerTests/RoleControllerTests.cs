using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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
                Name = "Admin"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(roles), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithUserClaims();

            //Act
            var response = await _client.SendAsync(request);

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}
