using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class ProfileControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _unAuthorisedClient = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authorisedClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetProfileUnauthorized()
    {
        var res = await _unAuthorisedClient.GetAsync("/api/profile", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProfile()
    {
        var res = await _authorisedClient.GetAsync("/api/profile", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Profile>();
        content.Should().BeEquivalentTo(new Profile("user@test.com", "", "", Domain.Enums.Age.Senior, false));
    }
}
