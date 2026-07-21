using System.Net.Http;
using System.Threading.Tasks;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Display;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class AccessControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _unAuthorisedClient = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authorisedClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetUnauthorised()
    {
        var res = await _unAuthorisedClient.GetAsync("/api/access/", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var accessModel = await res.DeserialiseAsync<AccessModel>();
        accessModel.Should().BeEquivalentTo(new AccessModel(false, false, [], [], [], []));
    }

    [Fact]
    public async Task GetAuthorised()
    {
        var res = await _authorisedClient.GetAsync("/api/access/", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var accessModel = await res.DeserialiseAsync<AccessModel>();
        accessModel.Should().BeEquivalentTo(new AccessModel(false, true, [1ul], [], [], []));
    }
}
