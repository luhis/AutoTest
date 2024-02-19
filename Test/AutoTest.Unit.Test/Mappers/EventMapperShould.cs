using AutoTest.Domain.Enums;
using AutoTest.Unit.Test.MockData;
using AutoTest.Web.Mapping;
using FluentAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Mappers
{
    public class EventMapperShould
    {
        [Fact]
        public void MapSaveModel()
        {
            var eventId = 1ul;
            var model = new Web.Models.EventSaveModel()
            {
                ClubId = 1,
                Location = "Kestrel Farm",
                CourseCount = 1,
                MaxAttemptsPerCourse = 1,
                MaxEntrants = 2,
                EventTypes = new[] { EventType.AutoTest }
            };
            var res = MapEvent.Map(eventId, model);

            var expected = Models.GetEvent(eventId, 1);

            res.Should().BeEquivalentTo(expected, o => o.Excluding(p => p.Created));
        }
    }
}
