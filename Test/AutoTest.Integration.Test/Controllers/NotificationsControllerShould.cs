using System;
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

public class NotificationsControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetNotifications()
    {
        var res = await _client.GetAsync($"/api/notifications/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Notification>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { NotificationId = TestIds.NotificationId, EventId = TestIds.EventId, Message = "test message" });
    }

    [Fact]
    public async Task AddReturnsUnauthorized()
    {
        var model = new NotificationSaveModel { Message = "new message", Created = DateTime.UtcNow };
        var res = await _client.PutAsJsonAsync($"/api/notifications/{TestIds.NotificationId}/{TestIds.EventId}", model, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
