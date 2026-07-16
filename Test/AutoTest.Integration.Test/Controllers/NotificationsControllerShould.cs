using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class NotificationsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient;

    public NotificationsControllerShould(CustomWebApplicationFactory<Startup> fixture)
    {
        unAuthorisedClient = fixture.GetUnAuthorisedClient();
    }

    [Fact]
    public async Task GetNotifications()
    {
        var res = await unAuthorisedClient.GetAsync("/api/notifications/22", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<IEnumerable<Notification>>();
        content.Should().NotBeEmpty();
    }
}
