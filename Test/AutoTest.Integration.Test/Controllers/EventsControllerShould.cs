using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Save;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class EventsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient unAuthorisedClient;
        private readonly HttpClient authorisedClient;

        public EventsControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2)
        {
            this.unAuthorisedClient = fixture.GetUnAuthorisedClient();
            this.authorisedClient = fixture2.GetAuthorisedClient();
        }

        [Fact]
        public async Task GetAll()
        {
            var res = await unAuthorisedClient.GetAsync("/api/events/");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.DeserialiseAsync<IEnumerable<Event>>();
            content.Should().NotBeEmpty();
        }

        [Fact]
        public async Task AddFailValidation()
        {
            var res = await authorisedClient.PutAsync("/api/events/22", JsonContent.Create(new EventSaveModel() { ClubId = 1 }));
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
            var content = await res.DeserialiseAsync<ProblemDetails>();
            content.Should().NotBeNull();
        }
    }
}
