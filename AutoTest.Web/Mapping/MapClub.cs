using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapClub
    {
        public static Club Map(ulong clubId, ClubSaveModel model) => new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);

        public static Event Map(ulong eventId, EventSaveModel @event) => new Event(eventId, @event.ClubId, @event.Location, @event.StartTime, @event.TestCount);

        public static Entrant Map(ulong entrantId, EntrantSaveModel entrant) => new Entrant(entrantId, entrant.GivenName, entrant.FamilyName, entrant.Registration, entrant.Class, entrant.EventId, entrant.IsPaid);
    }
}
