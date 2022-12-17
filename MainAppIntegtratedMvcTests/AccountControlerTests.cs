using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace MainAppIntegtratedMvcTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<ProgramMVC>>//wspoldzielenie factory testy nieco szybsze
    {
        private IConfigurationSection _authSettings;
        private HttpClient _httpClient;

        public AccountControllerTests(WebApplicationFactory<ProgramMVC> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2665
           // _authSettings = new ConfigurationBuilder()
           //.SetBasePath(Directory.GetCurrentDirectory())
           //.AddJsonFile("appsettings.json")
           //.Build()
           //.GetSection("ApiKey");

            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44331/");
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            //_httpClient.DefaultRequestHeaders.Add("ApiKey", _authSettings.Value);//TODDO dodac role!
        }

        [Fact]
        public async Task Get_UserWithAuthentication_ReturnStatusNotFound()
        {
            //arrange

            //act
            var response = await _httpClient.GetAsync("/User/100");

            //assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("/User")]
        public async Task GetAll_UsersEndPoints_WithAuthentication_ReturnStatusOk(string endpoint)
        {
            // Arrange
            var expectedStatusCode = HttpStatusCode.OK;
            var request = new HttpRequestMessage(HttpMethod.Get, $"{endpoint}");

            //Act
            var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
