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
            c.SetAdminEmails(model.AdminEmails.Select(a => new AuthorisationEmail(a.Email)).ToArray());
            return c;
        }

        public static Event Map(ulong eventId, EventSaveModel @event)
        {
            var e = new Event(eventId, @event.ClubId, @event.Location, @event.StartTime, @event.TestCount,
                @event.MaxAttemptsPerTest, @event.Regulations);
            e.SetMarshalEmails(@event.MarshalEmails.Select(a => new AuthorisationEmail(a.Email)).ToArray());
            return e;
        }

        private static EmergencyContact Map(EmergencyContactSaveModel emergencyContact)
        {
            return new EmergencyContact(emergencyContact.Name, emergencyContact.Phone);
        }

        public static Vehicle Map(VehicleSaveModel vehicle)
        {
            return new Vehicle(vehicle.Make, vehicle.Model, vehicle.Year,
                vehicle.Displacement, vehicle.Registration);
        }

        public static ClubMembership Map(ClubMembershipSaveModel vehicle)
        {
            return new ClubMembership(vehicle.ClubName, vehicle.MembershipNumber, vehicle.Expiry);
        }

        public static TestRun Map(ulong testRunId, TestRunSaveModel test)
        {
            var run = new TestRun(testRunId, test.EventId, test.Ordinal, test.TimeInMS, test.EntrantId, test.Created);
            run.SetPenalties(test.Penalties.Select(a => new Penalty(a.PenaltyType, a.InstanceCount)).ToArray());
            return run;
        }

        public static Entrant Map(ulong entrantId, ulong eventId, EntrantSaveModel entrant)
        {
            var e = new Entrant(entrantId, entrant.DriverNumber, entrant.GivenName, entrant.FamilyName, entrant.Class, eventId,
                entrant.IsPaid, entrant.Club);
            e.SetVehicle(Map(entrant.Vehicle));
            e.SetEmergencyContact(Map(entrant.EmergencyContact));
            return e;
        }

        public static Profile Map(string emailAddress, ProfileSaveModel profile)
        {
            var p = new Profile(emailAddress, profile.GivenName, profile.FamilyName, profile.MsaLicense);
            p.SetVehicle(Map(profile.Vehicle));
            p.SetEmergencyContact(Map(profile.EmergencyContact));
            p.SetClubMemberships(profile.ClubMemberships.Select(Map).ToArray());
            return p;
        }
    }
}
