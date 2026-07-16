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

public class TestRunsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient;

    public TestRunsControllerShould(CustomWebApplicationFactory<Startup> fixture)
    {
        unAuthorisedClient = fixture.GetUnAuthorisedClient();
    }

    [Fact]
    public async Task GetTestRuns()
    {
        var res = await unAuthorisedClient.GetAsync("api/events/22/tests/2/testRuns", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<TestRun>>();
        content.Should().NotBeEmpty();
    }
}
