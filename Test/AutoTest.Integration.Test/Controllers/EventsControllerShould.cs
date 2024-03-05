using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Integration.Test.Fixtures;
using AutoTest.Integration.Test.Tooling;
using AutoTest.Web;
using AutoTest.Web.Models.Save;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AutoTest.Integration.Test.Controllers
{
    public class EventsControllerShould : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<AuthdCustomWebApplicationFactory<Startup>>
    {
        private readonly MockRepository _mockRepo;
        private readonly Mock<IFileRepository> _fileRepo;
        private readonly HttpClient unAuthorisedClient;
        private readonly HttpClient authorisedClient;

        public EventsControllerShould(CustomWebApplicationFactory<Startup> fixture, AuthdCustomWebApplicationFactory<Startup> fixture2)
        {
            this._mockRepo = fixture.Mr;
            this._fileRepo = fixture.IFileRepository;
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

        [Fact]
        public async Task GetMaps()
        {
            _fileRepo.Setup(f => f.GetMaps(1, It.IsAny<CancellationToken>())).ReturnsAsync("");
            var res = await unAuthorisedClient.GetAsync("/api/events/1/maps");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();

            content.Should().NotBeNull();
            _mockRepo.VerifyAll();
        }

        [Fact]
        public async Task GetRegulations()
        {
            _fileRepo.Setup(f => f.GetRegs(1, It.IsAny<CancellationToken>())).ReturnsAsync("");
            var res = await unAuthorisedClient.GetAsync("/api/events/1/regulations");
            res.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var content = await res.Content.ReadAsStringAsync();

            content.Should().NotBeNull();
            _mockRepo.VerifyAll();
        }
    }
}
