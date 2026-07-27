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
using AutoTest.Web.Models.Save;
using AwesomeAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class EventsControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetAll()
    {
        var res = await _client.GetAsync("/api/events/", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Event>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new
            {
                EventId = TestIds.EventId,
                ClubId = TestIds.ClubId,
                Location = "",
                CourseCount = 10,
                MaxAttemptsPerCourse = 2,
                TimingSystem = TimingSystem.StopWatch,
                EventTypes = new[] { EventType.AutoTest }
            });
    }

    [Fact]
    public async Task SaveReturnsUnauthorized()
    {
        var res = await _client.PutAsJsonAsync($"/api/events/{TestIds.EventId}", new EventSaveModel { ClubId = TestIds.ClubId }, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteReturnsUnauthorized()
    {
        var res = await _client.DeleteAsync($"/api/events/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SaveFailValidation()
    {
        var res = await _authClient.PutAsJsonAsync($"/api/events/{TestIds.EventId}", new EventSaveModel() { ClubId = TestIds.ClubId }, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var content = await res.DeserialiseAsync<ProblemDetails>();
        content.Should().BeEquivalentTo(new { Status = 400, Title = "One or more validation errors occurred." });
    }

    [Fact]
    public async Task GetMaps()
    {
        var res = await _client.GetAsync($"/api/events/{TestIds.EventId}/maps", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("");
    }

    [Fact]
    public async Task GetRegulations()
    {
        var res = await _client.GetAsync($"/api/events/{TestIds.EventId}/regulations", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
        content.Should().Be("");
    }
}
