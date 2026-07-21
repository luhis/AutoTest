using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Save;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class ClubsControllerShould(TestWebApplicationFactory<Startup> factory) : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();

    [Fact]
    public async Task GetClubs()
    {
        var res = await _client.GetAsync("/api/clubs/", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Club>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { ClubId = TestIds.ClubId, ClubName = "BHMC", Website = "www.club.com" });
    }

    [Fact]
    public async Task PutReturnsUnauthorized()
    {
        var res = await _client.PutAsJsonAsync($"/api/clubs/{TestIds.ClubId}", new ClubSaveModel(), TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteReturnsUnauthorized()
    {
        var res = await _client.DeleteAsync($"/api/clubs/{TestIds.ClubId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
