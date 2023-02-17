using MainApp.BLL.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Web.Services
{
    public class EmailService
    {
        IHttpClientFactory httpClientFactory;
        private const string AppiUrl = "https://localhost:7148/api";

        public EmailService(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<Email> CreateEmail(Email model)
        {

            HttpClient client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AppiUrl}/Email");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            request.Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var result = await client.SendAsync(request);

            if (!result.IsSuccessStatusCode)
                return null;

            return model;
        }
    }
}
