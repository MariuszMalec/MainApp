using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Tracking;
using Tracking.Context;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerWithMemorySqlTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public TrainerControllerWithMemorySqlTests(WebApplicationFactory<Program> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MainApplicationContext>));

                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<MainApplicationContext>(options => options.UseInMemoryDatabase("TrainerDb"));

                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAll_Trainers_ReturnOk_WhenExist()
        {
            //arrange 
            //TODO patrz startup dodano tam seed trainera jesli baza nierelacyjna

            //act
            var response = await _client.GetAsync("/api/Trainer");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_Trainer_ReturnOk_WhenExist()
        {
            //act
            var response = await _client.DeleteAsync($"/api/Trainer/{1}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_Trainer_ReturnNotFound_WhenNotExist()
        {
            //act
            var response = await _client.DeleteAsync($"/api/Trainer/{2}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Trainer_ReturnOk_WhenExist()
        {
            //act
            var response = await _client.GetAsync($"/api/Trainer/{1}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Get_Trainer_ReturnNotFound_WhenNotExist()
        {
            //act
            var response = await _client.GetAsync($"/api/Trainer/{2}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

    }
}
