﻿using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Tracking.Context;
using Microsoft.Extensions.DependencyInjection;

namespace TrackingTests.UserControllerTests
{
    //Jak uderzac do mvc skoro tracking tez ma nazwe Program?? mvc => ApplicationDbContext/ tracking => MainApplicationContext
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("UnitTests");//TODO to dodalem aby poszly testy. Czemu nie dziala to co nizej!!
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MainApplicationContext>));

#pragma warning disable CS8604 // Possible null reference argument.
                        services.Remove(dbContextOptions);
#pragma warning restore CS8604 // Possible null reference argument.

                        ;
                        services
                          .AddDbContext<MainApplicationContext>(options => options.UseInMemoryDatabase("UsersDb"));//TODO to nie dziala musialem dodac w program.cs!

                    });
                })
                .CreateClient();

            _client.BaseAddress = new Uri("https://localhost:7001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(
                HeaderNames.Accept, "application/json");
            _client.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
        }

        [Theory]
        [InlineData("/api/User")]
        [InlineData("/api/User/1")]
        public async Task GetAll_Users_ReturnOk_WhenExist(string endpoint)
        {

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api/User/100")]
        [InlineData("/api/User/120")]
        public async Task GetAll_Users_ReturnBadRequest_WhenNotExist(string endpoint)
        {

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
