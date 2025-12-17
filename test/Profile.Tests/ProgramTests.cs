using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Profile.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Profile.Program>>
    {
        private readonly WebApplicationFactory<Profile.Program> _factory;

        public ProgramTests(WebApplicationFactory<Profile.Program> factory)
        {
            // Use solution relative content root so the factory can locate wwwroot
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseSolutionRelativeContentRoot("Profile");
            });
        }

        [Fact]
        public async Task GetRoot_ReturnsRedirectToIndex()
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
            var response = await client.GetAsync("/");

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/index.html", response.Headers.Location?.OriginalString);
        }

        [Fact]
        public async Task GetIndexHtml_ReturnsOkAndHtmlContent()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/index.html");

            response.EnsureSuccessStatusCode();
            var contentType = response.Content.Headers.ContentType?.ToString();
            Assert.NotNull(contentType);
            Assert.Contains("text/html", contentType);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("<!DOCTYPE html", content, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task GetFavicon_ReturnsSuccess()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/favicon.ico");

            response.EnsureSuccessStatusCode();
        }
    }
}
