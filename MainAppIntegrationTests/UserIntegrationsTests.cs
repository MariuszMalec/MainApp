using MainApp.Web;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MainAppIntegrationTests
{
    public class UserIntegrationsTests : IClassFixture<TestingWebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        public UserIntegrationsTests(TestingWebAppFactory<Program> factory)
            => _client = factory.CreateClient();


        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            var response = await _client.GetAsync("/Trainer");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Mark", responseString);
            Assert.Contains("Evelin", responseString);
        }

    }


}
