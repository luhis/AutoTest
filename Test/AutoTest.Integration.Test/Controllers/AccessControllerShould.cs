using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Display;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class AccessControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2) : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
{
    private readonly HttpClient unAuthorisedClient = fixture.GetUnAuthorisedClient();
    private readonly HttpClient authorisedClient = fixture2.GetAuthorisedClient();

    [Fact]
    public async Task GetUnauthorised()
    {
        var res = await unAuthorisedClient.GetAsync("/api/access/", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var accessModel = await res.DeserialiseAsync<AccessModel>();
        accessModel.Should().BeEquivalentTo(new AccessModel(false, false, [], [], [], []));
    }

    [Fact]
    public async Task GetAuthorised()
    {
        var res = await authorisedClient.GetAsync("/api/access/", CancellationToken.None);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var accessModel = await res.DeserialiseAsync<AccessModel>();
        accessModel.Should().BeEquivalentTo(new AccessModel(false, true, [1ul], [], [], []));
    }
}
