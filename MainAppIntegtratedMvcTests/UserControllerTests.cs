using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net;
using Tracking.Context;

namespace MainAppIntegtratedMvcTests
{
    public class UserControllerTests : IClassFixture<TestingWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();

        public UserControllerTests(TestingWebAppFactory<ProgramMVC> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
                             {
                                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
                             })
                             .CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Theory]
        [InlineData("/User")]
        public async Task Get_Users_EndPointsReturnsSuccessForAdmin(string url)
        {
            var provider = TestClaimsProvider.WithAdminClaims();
            _client = factory.CreateClientWithTestAuth(provider);

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
