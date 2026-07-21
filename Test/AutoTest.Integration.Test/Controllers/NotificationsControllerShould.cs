using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class NotificationsControllerShould(TestWebApplicationFactory<Startup> factory) : IClassFixture<TestWebApplicationFactory<Startup>>
{
    private readonly HttpClient _unAuthorisedClient = factory.GetUnAuthorisedClient();

    [Fact]
    public async Task GetNotifications()
    {
        var res = await _unAuthorisedClient.GetAsync($"/api/notifications/{TestIds.EventId}", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Notification>>();
        content.Should().NotBeEmpty().And.ContainSingle()
            .Which.Should().BeEquivalentTo(new { NotificationId = TestIds.NotificationId, EventId = TestIds.EventId, Message = "test message" });
    }
}
