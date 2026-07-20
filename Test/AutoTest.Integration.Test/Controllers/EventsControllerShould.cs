using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Save;
using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class EventsControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2) : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();
    private readonly HttpClient authorisedClient = fixture2.GetAuthorisedClient();

    [Fact]
    public async Task GetAll()
    {
        var res = await unAuthorisedClient.GetAsync("/api/events/", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Event>>();
        content.Should().NotBeEmpty();
    }

    [Fact]
    public async Task AddFailValidation()
    {
        var res = await authorisedClient.PutAsync($"/api/events/{TestIds.EventId}", JsonContent.Create(new EventSaveModel() { ClubId = TestIds.ClubId }), TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        var content = await res.DeserialiseAsync<ProblemDetails>();
        content.Status.Should().Be(400);
        content.Title.Should().Be("One or more validation errors occurred.");
    }

    [Fact]
    public async Task GetMaps()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/events/{TestIds.ClubId}/maps", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("");
    }

    [Fact]
    public async Task GetRegulations()
    {
        var res = await unAuthorisedClient.GetAsync($"/api/events/{TestIds.ClubId}/regulations", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("");
    }
}
