using FluentAssertions;
using MainApp.BLL.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tracking;
using Tracking.Context;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerTests : IClassFixture<WebApplicationFactory<Startup>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public TrainerControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<MainApplicationContext>));

                        services.Remove(dbContextOptions);

                        services
                         .AddDbContext<MainApplicationContext>(options => options.UseInMemoryDatabase("UserDb"));

                    });
                })
                .CreateClient();
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
