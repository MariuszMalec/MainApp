﻿using MainApp.BLL.Context;
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
    public class AccountControllerTests : IClassFixture<TestingWebAppFactory<ProgramMVC>>
    {
        private HttpClient _client;

        WebApplicationFactory<ProgramMVC> factory = new WebApplicationFactory<ProgramMVC>();

        public AccountControllerTests(TestingWebAppFactory<ProgramMVC> factory)
        {
            _client = factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.Timeout = new TimeSpan(0, 0, 30);
            _client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
        }

        [Fact]
        public async Task Register_ReturnsSuccess_WhenStatusRedirect()
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
            _client = factory.CreateClientWithTestAuth(provider);

            var response = await _client.SendAsync(request);

            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
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
            _client = factory.CreateClientWithTestAuth(provider);

            var response = await _client.SendAsync(request);

            Assert.IsType<HttpResponseMessage>(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

    }
}