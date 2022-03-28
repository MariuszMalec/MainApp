using MainApp.BLL.Entities;
using MainApp.BLL.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class UserToApiService
    {
        private readonly ILogger<UserToApiService> _logger;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        public UserToApiService(ILogger<UserToApiService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task CreateUser(RegisterView request, User user)
        {
            HttpClient client = httpClientFactory.CreateClient();

            var model = new User() { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, CreatedDate = DateTime.Now, PhoneNumber = request.PhoneNumber, PasswordHash = user.PasswordHash };

            var requestUser = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/User");

            requestUser.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            requestUser.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(requestUser);
        }

    }
}
