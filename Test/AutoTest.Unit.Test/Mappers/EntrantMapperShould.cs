using AutoTest.Domain.StorageModels;
using AutoTest.Web.Mapping;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Mappers
{
    public class EntrantMapperShould
    {
        [Fact]
        public void MapSaveModel()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var model = new Web.Models.EntrantSaveModel();
            var res = MapClub.Map(entrantId, eventId, model, "a@a.com");

            var expected = new Entrant(entrantId, model.DriverNumber, model.GivenName, model.FamilyName, "a@a.com", model.Class, eventId, model.Age, model.IsLady);

            res.Should().BeEquivalentTo(expected);
        }
    }
}
