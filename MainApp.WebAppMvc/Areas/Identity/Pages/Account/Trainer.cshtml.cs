// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using MainApp.WebAppMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace MainApp.WebAppMvc.Areas.Identity.Pages.Account
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [AllowAnonymous]
    public class TrainerModel : PageModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>

        private const string AppiUrl = "https://localhost:7001/api";
        private readonly IHttpClientFactory httpClientFactory;

        public TrainerModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task OnGet()
        {                 
            HttpClient client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("ApiKey", "8e421ff965cb4935ba56ef7833bf4750");
            var answer = await client.SendAsync(request);
            if (!answer.IsSuccessStatusCode)
            {
                ViewData["Trainers"] = "Blad !";
            }
            var content = answer.Content.ReadAsStringAsync().Result;
            var trainers = JsonConvert.DeserializeObject<List<TrainerView>>(content);
            if (trainers.Count > 0)
            {
                ViewData["Trainers"] = trainers;
                ViewData["TrainersAsString"] = content;
            }
            else
            {
                ViewData["Trainers"] = "brak trenerow!";
            }
        }
    }
}
