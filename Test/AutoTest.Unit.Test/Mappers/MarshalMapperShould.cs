using AutoTest.Domain.StorageModels;
using AutoTest.Web.Mapping;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Mappers
{
    public class MarshalMapperShould
    {
        [Fact]
        public void MapSaveModel()
        {
            var entrantId = 1ul;
            var eventId = 2ul;
            var model = new Web.Models.MarshalSaveModel();
            var res = MapMarshal.Map(entrantId, eventId, model, "a@a.com");

            var expected = new Marshal(entrantId, model.GivenName, model.FamilyName, "a@a.com", eventId, 0, "");

            res.Should().BeEquivalentTo(expected);
        }
    }
}
