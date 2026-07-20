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

public class MarshalsControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2) : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();
    private readonly HttpClient authorisedClient = fixture2.GetAuthorisedClient();

    [Fact]
    public async Task GetMarshals()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/marshals/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<PublicMarshalModel>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { GivenName = "Dave", FamilyName = "Marshal" });
    }

    [Fact]
    public async Task GetSingle()
    {
        var res = await authorisedClient.GetAsync($"/api/marshals/{TestIds.EventId}/{TestIds.MarshalId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Marshal>();
        content.Should().BeEquivalentTo(new Marshal(TestIds.MarshalId, "Dave", "Marshal", "test@test.com", TestIds.EventId, 123, "role"));
    }

    [Fact]
    public async Task NotFound()
    {
        var res = await authorisedClient.GetAsync($"/api/marshals/{TestIds.EventId}/9999", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
