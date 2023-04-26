using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace MainAppIntegtratedMvcTests
{
    public class UserControllerTests : IClassFixture<TestingWebAppFactory<ProgramMVC>> //TODO musi byc uruchomiony project tracking! mvc strzela do api aby zarejstrowac event
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();

        public UserControllerTests(TestingWebAppFactory<ProgramMVC> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Theory]
        [InlineData("/User")]
        public async Task Get_Users_EndPointsReturnsSuccessForAdmin(string url)
        {
            var provider = TestClaimsProvider.WithAdminClaims();

            //_client = factory.CreateClientWithTestAuth(provider);//TODO odpala ponownie programmvc co wywala test

            var response = await _client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
