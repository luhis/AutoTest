using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Display;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class EntrantsControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _unAuthorisedClient = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authorisedClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetEntrants()
    {
        var res = await _unAuthorisedClient.GetAsync($"/api/entrants/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<PublicEntrantModel>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { GivenName = "Dave", FamilyName = "Entrant" });
    }

    [Fact]
    public async Task GetSingle()
    {
        var res = await _authorisedClient.GetAsync($"/api/entrants/{TestIds.EventId}/{TestIds.EntrantId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Entrant>();
        content.Should().BeEquivalentTo(new Entrant(TestIds.EntrantId, 2, "Dave", "Entrant", "test@test.com", "A", TestIds.EventId, Domain.Enums.Age.Senior, false, null));
    }

    [Fact]
    public async Task NotFound()
    {
        var res = await _authorisedClient.GetAsync($"/api/entrants/{TestIds.EventId}/9999", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
