using System.Linq;
using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapClub
    {
        public static Club Map(ulong clubId, ClubSaveModel model)
        {
            var c = new Club(clubId, model.ClubName, model.ClubPaymentAddress, model.Website);
            c.SetAdminEmails(model.AdminEmails.Select(a => new AuthorisationEmail(a)).ToArray());
            return c;
        }

        public static Event Map(ulong eventId, EventSaveModel @event)
        {
            var e = new Event(eventId, @event.ClubId, @event.Location, @event.StartTime, @event.TestCount,
                @event.MaxAttemptsPerTest);
            e.SetMarshalEmails(@event.MarshalEmails.Select(a => new AuthorisationEmail(a)).ToArray());
            return e;
        }

        public static TestRun Map(ulong testRunId, TestRunSaveModel test)
        {
            var run = new TestRun(testRunId, test.TestId, test.TimeInMS, test.EntrantId, test.Created);
            run.SetPenalties(test.Penalties.Select(a => new Penalty(a.PenaltyType, a.InstanceCount)).ToArray());
            return run;
        }

        public static Entrant Map(ulong entrantId, EntrantSaveModel entrant)
        {
            var e = new Entrant(entrantId, entrant.DriverNumber, entrant.GivenName, entrant.FamilyName, entrant.Class, entrant.EventId,
                entrant.IsPaid);
            e.SetVehicle(new Vehicle(entrant.Vehicle.Make, entrant.Vehicle.Model, entrant.Vehicle.Year, entrant.Vehicle.Displacement, entrant.Vehicle.Registration));
            return e;
        }
    }
}
