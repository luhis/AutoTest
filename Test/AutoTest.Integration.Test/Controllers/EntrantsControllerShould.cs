using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Display;
using AutoTest.Web.Models.Save;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class EntrantsControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetEntrants()
    {
        var res = await _client.GetAsync($"/api/entrants/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<PublicEntrantModel>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                EntrantId = TestIds.EntrantId,
                GivenName = "Dave",
                FamilyName = "Entrant",
                EventId = TestIds.EventId,
                Class = "A",
                IsLady = false
            });
    }

    [Fact]
    public async Task GetSingle()
    {
        var res = await _authClient.GetAsync($"/api/entrants/{TestIds.EventId}/{TestIds.EntrantId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Entrant>();
        content.Should().BeEquivalentTo(new Entrant(TestIds.EntrantId, 2, "Dave", "Entrant", "test@test.com", "A", TestIds.EventId, Age.Senior, false, null));
    }

    [Fact]
    public async Task GetSingleReturnsUnauthorized()
    {
        var res = await _client.GetAsync($"/api/entrants/{TestIds.EventId}/{TestIds.EntrantId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSingleNotFound()
    {
        var res = await _authClient.GetAsync($"/api/entrants/{TestIds.EventId}/9999", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutReturnsUnauthorized()
    {
        var res = await _client.PutAsJsonAsync($"/api/entrants/{TestIds.EventId}/{TestIds.EntrantId}", new EntrantSaveModel(), TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteReturnsUnauthorized()
    {
        var res = await _client.DeleteAsync($"/api/entrants/{TestIds.EventId}/{TestIds.EntrantId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
