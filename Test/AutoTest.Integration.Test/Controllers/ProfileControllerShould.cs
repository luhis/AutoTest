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

public class ProfileControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2) : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();
    private readonly HttpClient authorisedClient = fixture2.GetAuthorisedClient();

    [Fact]
    public async Task GetProfileUnauthorized()
    {
        var res = await unAuthorisedClient.GetAsync("/api/profile", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProfile()
    {
        var res = await authorisedClient.GetAsync("/api/profile", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Profile>();
        content.Should().BeEquivalentTo(new Profile("user@test.com", "", "", Domain.Enums.Age.Senior, false));
    }
}
