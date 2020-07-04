using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapClub
    {
        public static Club Map(ulong clubId, ClubSaveModel model) => new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);

        public static Event Map(ulong eventId, EventSaveModel @event) => new Event(eventId, @event.ClubId, @event.Location, @event.StartTime, @event.TestCount, @event.MaxAttemptsPerTest);
        public static TestRun Map(ulong eventId, TestRunSaveModel test) => new TestRun(eventId, test.TestId, test.TimeInMS, test.EntrantId, test.Created);

        public static Entrant Map(ulong entrantId, EntrantSaveModel entrant)
        {
            var e = new Entrant(entrantId, entrant.GivenName, entrant.FamilyName, entrant.Class, entrant.EventId,
                entrant.IsPaid);
            e.SetVehicle(new Vehicle(entrant.Vehicle.VehicleId, entrant.Vehicle.Make, entrant.Vehicle.Model, entrant.Vehicle.Year, entrant.Vehicle.Displacement, entrant.Vehicle.Registration));
            return e;
        }
    }
}
