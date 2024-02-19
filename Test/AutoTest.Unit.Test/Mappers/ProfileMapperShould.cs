using AutoTest.Domain.StorageModels;
using AutoTest.Web.Mapping;
using AutoTest.Web.Models.Save;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Mappers
{
    public class ProfileMapperShould
    {
        [Fact]
        public void MapSaveModel()
        {
            var model = new ProfileSaveModel();
            var res = MapProfile.Map("a@a.com", model);

            var expected = new Profile("a@a.com", "", "", Domain.Enums.Age.Senior, false);

            res.Should().BeEquivalentTo(expected);
        }
    }
}
