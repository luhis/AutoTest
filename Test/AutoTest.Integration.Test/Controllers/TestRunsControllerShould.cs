using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class TestRunsControllerShould(TestWebApplicationFactory<Startup> factory) : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();

    [Fact]
    public async Task GetTestRuns()
    {
        var res = await _client.GetAsync($"/api/events/{TestIds.EventId}/tests/{TestIds.TestNumber}/testRuns", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<TestRun>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { TestRunId = TestIds.TestRunId, EventId = TestIds.EventId, Ordinal = TestIds.TestNumber });
    }

    [Fact]
    public async Task PutReturnsUnauthorized()
    {
        var res = await _client.PutAsJsonAsync($"/api/events/{TestIds.EventId}/tests/{TestIds.TestNumber}/testRuns/{TestIds.TestRunId}", new { }, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
