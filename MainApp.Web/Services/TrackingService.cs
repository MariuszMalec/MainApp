using MainApp.BLL.Entities;
using MainApp.BLL.Enums;
using MainApp.BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class TrackingService
    {
        private readonly ILogger<TrackingService> _logger;
        private readonly UserService _userService;
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:44311/api";

        public TrackingService(ILogger<TrackingService> logger, IHttpClientFactory httpClientFactory, UserService userService)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
            _userService = userService;
        }
        public async Task Insert(Event myEvent)
        {
            HttpClient client = httpClientFactory.CreateClient();

            var requestUser = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Tracking");

            requestUser.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            requestUser.Content = new StringContent(JsonConvert.SerializeObject(myEvent), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(requestUser);
        }

        public async Task<Event> InsertEvent(ActivityActions activityActions, HttpContext httpContext, string email)
        {
            var userEmail = httpContext.User.Identity.Name;
            if (userEmail == null)
                userEmail = email;
            var user = await _userService.GetByEmail(userEmail);
            return new Event { CreatedDate = DateTime.UtcNow, User = user, Email = userEmail, Action = activityActions.ToString()};
        }


    }


}
