using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrackingIntegrationsTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string AppiUrl = "https://localhost:7001/api";

        public TrackingIntegrationsTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();

        [Fact]
        public async Task Index_ReturnsTrainers_WhenStatusOK()
        {
            var response = await _client.GetAsync($"{AppiUrl}/Trainer");
            //response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }


}
