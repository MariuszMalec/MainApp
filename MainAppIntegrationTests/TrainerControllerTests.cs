using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerTests : IClassFixture<WebApplicationFactory<Startup>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public TrainerControllerTests(WebApplicationFactory<Startup> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2665
            _client = factory.CreateClient();//to dziala ale musi byc polaczenie z baza
        }

        [Fact]
        public async Task GetAll_Trainers_ReturnOk()
        {

            //act
            var response = await _client.GetAsync("/api/Trainer");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
