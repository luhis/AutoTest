using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Service.Models;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class ResultsControllerShould(CustomWebApplicationFactory<Startup> fixture) : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();

    [Fact]
    public async Task GetResults()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/results/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Result>>();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task GetAwards()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/results/{TestIds.EventId}/awards", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Awards>();
        content.Should().NotBeNull();
        content!.Ftd.Should().NotBeNull();
        content.ClassAwards.Should().NotBeNull();
    }
}
