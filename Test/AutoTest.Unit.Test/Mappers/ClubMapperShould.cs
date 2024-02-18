using AutoTest.Domain.StorageModels;
using AutoTest.Web.Mapping;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Mappers
{
    public class ClubMapperShould
    {
        [Fact]
        public void MapSaveModel()
        {
            var clubId = 1ul;
            var model = new Web.Models.ClubSaveModel();
            var res = MapClub.Map(clubId, model);

            var expected = new Club(clubId, "", "", "");

            res.Should().BeEquivalentTo(expected);
        }
    }
}
