﻿using FluentAssertions;
using MainApp.BLL.Context;
using MainApp.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MainAppIntegrationTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public UserControllerTests(WebApplicationFactory<Startup> factory, IServiceScope serviceScope)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("UserDb"));
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAll_Users_ReturnOk()
        {

            //act
            var response = await _client.GetAsync("/api/User");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
