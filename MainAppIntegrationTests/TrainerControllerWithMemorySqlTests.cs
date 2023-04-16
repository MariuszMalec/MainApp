using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Net.Http.Headers;
using System;
using System.Data.Common;
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

                        var dbConnectionDesciptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbConnection));

                        services.Remove(dbConnectionDesciptor);

                        services
                         .AddDbContext<MainApplicationContext>(options => options.UseInMemoryDatabase("TrackingDb"));//TODO to nie dziala musialem dodac w program.cs!

                        services.AddHttpClient("Tracking", client =>
                        {
                            client.BaseAddress = new Uri("https://localhost:7001/");
                            client.Timeout = new TimeSpan(0, 0, 30);
                            client.DefaultRequestHeaders.Add(
                                HeaderNames.Accept, "application/json");
                            client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");//TODO tutaj to nie dziala?
                        });

                    });

                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy.
                })
                .CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
        }

        [Theory]
        [InlineData("/api/Trainer")]
        [InlineData("/api/Trainer/1")]
        public async Task GetAll_Trainers_ReturnOk_WhenExist(string endpoint)
        {
            //arrange 
            //TODO patrz startup dodano tam seed trainera jesli baza nierelacyjna

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/Trainer/80")]
        [InlineData("/api/Trainer/100")]
        public async Task Get_Trainer_ReturnNotFound_WhenNotExist(string endpoint)
        {
            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Trainer_ReturnNotFound_WhenNotExist()
        {
            //act
            var response = await _client.DeleteAsync($"/api/Trainer/{2}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }


    }
}
