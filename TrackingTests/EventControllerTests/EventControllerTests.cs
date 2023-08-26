using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tracking.Context;

namespace TrackingTests.EventControllerTests
{
    public class EventControllerWithMemorySqlTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public EventControllerWithMemorySqlTests(WebApplicationFactory<Program> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MainApplicationContext>));

#pragma warning disable CS8604 // Possible null reference argument.
                        services.Remove(dbContextOptions);
#pragma warning restore CS8604 // Possible null reference argument.

                        services
                         .AddDbContext<MainApplicationContext>(options => options.UseInMemoryDatabase("EventDb"));//TODO Czemu nie dziala to!!

                    });
                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAll_Events_ReturnUnauthorized_WhenExist()
        {

            //act
            var response = await _client.GetAsync("/api/Tracking");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetEvent_Events_ReturnUnauthorized_WhenNotExist()
        {
            //arrange
            await _client.DeleteAsync($"/api/Tracking/{1}");

            //act
            var response = await _client.GetAsync("/api/Tracking");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
