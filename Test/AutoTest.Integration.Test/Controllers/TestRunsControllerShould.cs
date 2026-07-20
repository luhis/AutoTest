using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class TestRunsControllerShould(CustomWebApplicationFactory<Startup> fixture) : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();

    [Fact]
    public async Task GetTestRuns()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/events/{TestIds.EventId}/tests/{TestIds.TestNumber}/testRuns", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<TestRun>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { TestRunId = TestIds.TestRunId, EventId = TestIds.EventId, Ordinal = TestIds.TestNumber });
    }
}
