
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace server_dotnet.tests {

    public class HealthEndpointTests(WebApplicationFactory<Program> factory)
        : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task GET_HealthEndpoint_ReturnsStatusUp()
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync("/health");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var payload = await response.Content.ReadFromJsonAsync<HealthDto>();
            payload.Should().NotBeNull();
            payload!.Status.Should().Be("UP");
        }

        private class HealthDto
        {
            public string? Status { get; set; }
        }
    }
}