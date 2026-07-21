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
using Xunit;

namespace AutoTest.Integration.Test.Controllers;

public class ProfileControllerShould(TestWebApplicationFactory<Startup> factory, AuthenticatedWebApplicationFactory<Startup> authenticatedFactory) : IClassFixture<TestWebApplicationFactory<Startup>>, IClassFixture<AuthenticatedWebApplicationFactory<Startup>>
{
    private readonly HttpClient _client = factory.GetUnAuthorisedClient();
    private readonly HttpClient _authClient = authenticatedFactory.GetAuthorisedClient();

    [Fact]
    public async Task GetProfileReturnsUnauthorized()
    {
        var res = await _client.GetAsync("/api/profile", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetProfile()
    {
        var res = await _authClient.GetAsync("/api/profile", TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Profile>();
        content.Should().NotBeNull();
        content!.EmailAddress.Should().Be("user@test.com");
    }

    [Fact]
    public async Task SaveProfile()
    {
        var model = new ProfileSaveModel
        {
            GivenName = "Updated",
            FamilyName = "User",
            Age = Age.Senior,
            IsLady = false,
            Vehicle = new VehicleSaveModel { Make = "Vauxhall", Model = "Corsa", Registration = "AB12 CDE", Displacement = 1200, Induction = Domain.Enums.Induction.NA },
            EmergencyContact = new EmergencyContactSaveModel { Name = "Emergency Contact", Phone = "01234567890" },
            MsaMembership = new MsaMembershipSaveModel { MsaLicense = 12345, MsaLicenseType = "Competition" },
            ClubMemberships = []
        };
        var res = await _authClient.PutAsJsonAsync("/api/profile", model, TestContext.Current.CancellationToken);
        res.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await res.DeserialiseAsync<Profile>();
        content.Should().BeEquivalentTo(new Profile("user@test.com", "Updated", "User", Age.Senior, false));
    }
}
