using MainApp.BLL.Context;
using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Tracking.Context;

namespace MainAppIntegtratedMvcTests
{
    public class AccountControllerTests : IClassFixture<TestingMainAppWebAppFactory<ProgramMVC>>, IClassFixture<TestingTrackingWebAppFactory<Program>>
    {
        private HttpClient _client;
        private HttpClient _client2;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();
        WebApplicationFactory<Program> factory2 = new WebApplicationFactory<Program>();

        public AccountControllerTests(TestingMainAppWebAppFactory<ProgramMVC> factory, TestingTrackingWebAppFactory<Program> factory2)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");

            _client2 = factory2.CreateClient();
            _client2.BaseAddress = new Uri("https://localhost:7001/");
            _client2.Timeout = new TimeSpan(0, 0, 30);
            _client2.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            _client2.DefaultRequestHeaders.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
        }

        [Theory]
        [InlineData("/api/Tracking")]
        public async Task GetEvent_ReturnsSuccess_WhenStatusOk(string endpoint)//TODO musi byc uruchomiony project tracking! mvc strzela do api aby zarejstrowac event
        {
            var response = await _client2.GetAsync($"{endpoint}");

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            Assert.Equal("/api/Tracking", requestMessage.RequestUri.LocalPath);
            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnsSuccess_WhenStatusRedirect()//TODO musi byc uruchomiony project tracking! mvc strzela do api aby zarejstrowac event
        {
            //TODO jak sprawdzic ze dobrze zarejstrowalo uzytkownika, event jest i wywala exception?! , jak sprawdzac model view??
            //TODO tylko redirect to sukses reszta to brak rejestracji
            var user = new RegisterView()
            {
                FirstName = "Test1",
                LastName = "Test1",
                PhoneNumber = "555-555-555",
                Password="123456",
                ConfirmPassword = "123456",
                Email = "tt2@example.com"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Account/Register");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithUserClaims();

            var response = await _client.SendAsync(request);

            var requestMessage = Assert.IsType<HttpRequestMessage>(response.RequestMessage);

            Assert.Equal("/Account/Login", requestMessage.RequestUri.LocalPath);
            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Register_ReturnsNotSuccess_WhenStatusOk()
        {
            var user = new RegisterView()
            {
                FirstName = "Test1",
                LastName = "Test1",
                PhoneNumber = "555-555-555",
                Password = "123456",
                ConfirmPassword = "123456",
                Email = "tt4@example.com"
            };

            var request = new HttpRequestMessage(HttpMethod.Post, $"/Account/Register");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var provider = TestClaimsProvider.WithUserClaims();
            //_client = factory.CreateClientWithTestAuth(provider);

            var response = await _client.SendAsync(request);

            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
