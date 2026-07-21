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

public class MarshalsControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetMarshals()
    {
        var res = await _client.GetAsync($"/api/marshals/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<PublicMarshalModel>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                MarshalId = TestIds.MarshalId,
                GivenName = "Dave",
                FamilyName = "Marshal",
                EventId = TestIds.EventId,
                Role = "role"
            });
    }

    [Fact]
    public async Task GetSingle()
    {
        var res = await _authClient.GetAsync($"/api/marshals/{TestIds.EventId}/{TestIds.MarshalId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Marshal>();
        content.Should().BeEquivalentTo(new Marshal(TestIds.MarshalId, "Dave", "Marshal", "test@test.com", TestIds.EventId, 123, "role"));
    }

    [Fact]
    public async Task GetSingleReturnsUnauthorized()
    {
        var res = await _client.GetAsync($"/api/marshals/{TestIds.EventId}/{TestIds.MarshalId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetSingleNotFound()
    {
        var res = await _authClient.GetAsync($"/api/marshals/{TestIds.EventId}/9999", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutReturnsUnauthorized()
    {
        var res = await _client.PutAsJsonAsync($"/api/marshals/{TestIds.EventId}/{TestIds.MarshalId}", new MarshalSaveModel(), TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteReturnsUnauthorized()
    {
        var res = await _client.DeleteAsync($"/api/marshals/{TestIds.EventId}/{TestIds.MarshalId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
