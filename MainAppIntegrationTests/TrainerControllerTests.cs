using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MainAppIntegrationTests
{
    public class TrainerControllerTests : IntegrationTest
    {
        private const string AppiUrl = "https://localhost:7001/api";

        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, $"{AppiUrl}/Trainer");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var result = await _httpClient.SendAsync(request);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
