﻿using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
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

                        services.Remove(dbContextOptions);

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
