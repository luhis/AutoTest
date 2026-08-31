using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Web;
using AwesomeAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class OpenApiShould(TestWebApplicationFactory<Startup> factory) : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.WithWebHostBuilder(builder =>
    {
        builder.UseEnvironment("Development");
    }).CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task ReturnValidOpenApiDocument()
    {
        var res = await _client.GetAsync("/openapi/v1.json", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Contain("\"openapi\"");
        content.Should().Contain("\"info\"");
        content.Should().Contain("\"paths\"");
    }
}
