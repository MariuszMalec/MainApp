using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tracking;
using Tracking.Context;
using Tracking.Services;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerWithMemorySqlTests : IClassFixture<WebApplicationFactory<Startup>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public TrainerControllerWithMemorySqlTests(WebApplicationFactory<Startup> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            //_httpClient = new() { BaseAddress = new Uri("https://localhost:7133") };


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

                        services.AddTransient<TrackingService>();

                        services.AddHttpClient();
                        services.AddHttpClient("Tracking", client =>
                        {
                            client.BaseAddress = new Uri("https://localhost:7001/");
                            client.Timeout = new TimeSpan(0, 0, 30);
                            client.DefaultRequestHeaders.Add(
                                HeaderNames.Accept, "application/json");
                            client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
                        });

                        MvcServiceCollectionExtensions.AddMvc(services, options => options.Filters.Add(new AllowAnonymousFilter()));

                        //services.AddAuthorization();



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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Trainer_ReturnOk_WhenExist()
        {
            //act
            var response = await _client.DeleteAsync($"/api/Trainer/{1}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Trainer_ReturnNotFound_WhenNotExist()
        {
            //act
            var response = await _client.DeleteAsync($"/api/Trainer/{2}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Trainer_ReturnOk_WhenExist()
        {
            //act
            var response = await _client.GetAsync($"/api/Trainer/{1}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Trainer_ReturnNotFound_WhenNotExist()
        {
            //act
            var response = await _client.GetAsync($"/api/Trainer/{2}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

    }
}
