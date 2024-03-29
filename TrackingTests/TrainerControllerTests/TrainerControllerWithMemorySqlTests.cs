﻿using FluentAssertions;
using MainApp.BLL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Data.Common;
using Tracking.Context;

namespace TrackingTests.TrainerControllerTests
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

#pragma warning disable CS8604 // Possible null reference argument.
                        services.Remove(dbContextOptions);
#pragma warning restore CS8604 // Possible null reference argument.

                        var dbConnectionDesciptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbConnection));

#pragma warning disable CS8604 // Possible null reference argument.
                        services.Remove(dbConnectionDesciptor);
#pragma warning restore CS8604 // Possible null reference argument.

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

        [Fact]
        public async Task GetAll_Trainers_ReturnTrainers_WhenExist()//only if env unit test
        {
            //act
            var response = await _client.GetAsync($"/api/Trainer");

            var content = await response.Content.ReadAsStringAsync();

            var trainers = JsonConvert.DeserializeObject<List<TrainerView>>(content);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
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
