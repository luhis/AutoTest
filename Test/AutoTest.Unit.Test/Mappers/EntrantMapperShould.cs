using AutoTest.Domain.StorageModels;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models.Save;
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
            var model = new EntrantSaveModel();
            var res = MapEntrant.Map(entrantId, eventId, model, "a@a.com");

            var expected = new Entrant(entrantId, model.DriverNumber, model.GivenName, model.FamilyName, "a@a.com", model.Class, eventId, model.Age, model.IsLady);

            res.Should().BeEquivalentTo(expected);
        }
    }
}
