using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Xunit;

namespace MainAppIntegrationTests
{
    public class ResponseIntegratedTest : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string AppiUrl = "https://localhost:7001/api";

        public ResponseIntegratedTest(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task Index_ReturnsWrongResponse_WhenIsNotStatusOK()
        {
            // Arrange
            var response = await _client.GetAsync($"{AppiUrl}/Trainer");//TODO zmien na enpoint ktorego nie ma w api aby wywalilo test
            //response.EnsureSuccessStatusCode();

            // Act
            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }


    }
}
