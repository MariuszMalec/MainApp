using FluentAssertions;
using MainApp.BLL.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MainAppUnitTests.TrackingControllerTests
{
    public class TrackingControllerTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;

        public TrackingControllerTests(WebApplicationFactory<Program> factory)
        {
            //https://youtu.be/6keSabBQRdE?t=2953

            _client = factory
                .WithWebHostBuilder(builder =>
                {
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
        [InlineData("/api/Tracking")]
        public async Task GetAll_Events_ReturnOk_WhenExist(string endpoint)
        {
            //arrange 
            //TODO patrz startup dodano tam seed trainera jesli baza nierelacyjna

            //act
            var response = await _client.GetAsync($"{endpoint}");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/api")]
        public async Task Insert_Event_ReturnOk_WhenExist(string endpoint)
        {
            //arrange 
            var newEvent = new Tracking.Models.Event { Action = ActivityActions.create.ToString(), CreatedDate = DateTime.Now, UserId = 1, Email = "Admin@example.com" };

            var request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}/Tracking");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(newEvent), Encoding.UTF8, "application/json");

            //act
            var result = await _client.SendAsync(request);

            //assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}
