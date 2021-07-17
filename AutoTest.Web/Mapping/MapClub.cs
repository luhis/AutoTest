using System.Linq;
using AutoTest.Domain.Enums;
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
                @event.MaxAttemptsPerTest, @event.Regulations, @event.EventType, @event.Maps, TimingSystem.StopWatch);
            return e;
        }

        private static EmergencyContact Map(EmergencyContactSaveModel emergencyContact)
        {
            return new EmergencyContact(emergencyContact.Name, emergencyContact.Phone);
        }

        private static MsaMembership Map(MsaMembershipSaveModel emergencyContact)
        {
            return new MsaMembership(emergencyContact.MsaLicenseType, emergencyContact.MsaLicense);
        }

        public static Vehicle Map(VehicleSaveModel vehicle)
        {
            return new Vehicle(vehicle.Make, vehicle.Model, vehicle.Year,
                vehicle.Displacement, vehicle.Induction, vehicle.Registration);
        }

        public static ClubMembership Map(ClubMembershipSaveModel membership)
        {
            return new ClubMembership(membership.ClubName, membership.MembershipNumber, membership.Expiry);
        }

        //public static TestRun Map(ulong eventId, int ordinal, ulong testRunId, TestRunSaveModel test)
        //{
        //    var run = new TestRun(testRunId, eventId, ordinal, test.TimeInMS, test.EntrantId, test.Created, 0);
        //    run.SetPenalties(test.Penalties.Select(a => new Penalty(a.PenaltyType, a.InstanceCount)).ToArray());
        //    return run;
        //}

        public static Entrant Map(ulong entrantId, ulong eventId, EntrantSaveModel entrant, string email)
        {
            var e = new Entrant(entrantId, entrant.DriverNumber, entrant.GivenName, entrant.FamilyName, email, entrant.Class, eventId,
                entrant.IsPaid, entrant.Club, entrant.ClubNumber, entrant.Age);
            e.SetVehicle(Map(entrant.Vehicle));
            e.SetEmergencyContact(Map(entrant.EmergencyContact));
            e.SetMsaMembership(Map(entrant.MsaMembership));
            return e;
        }

        public static Marshal Map(ulong marshalId, ulong eventId, MarshalSaveModel entrant, string email)
        {
            var e = new Marshal(marshalId, entrant.GivenName, entrant.FamilyName, email, eventId,
                entrant.RegistrationNumber, entrant.Role);
            e.SetEmergencyContact(Map(entrant.EmergencyContact));
            return e;
        }

        public static Profile Map(string emailAddress, ProfileSaveModel profile)
        {
            var p = new Profile(emailAddress, profile.GivenName, profile.FamilyName, profile.Age);
            p.SetVehicle(Map(profile.Vehicle));
            p.SetEmergencyContact(Map(profile.EmergencyContact));
            p.SetClubMemberships(profile.ClubMemberships.Select(Map).ToArray());
            p.SetMsaMembership(Map(profile.MsaMembership));
            return p;
        }

        public static Notification Map(ulong notificationId, ulong eventId, string emailAddress, NotificationSaveModel notification)
        {
            var p = new Notification(notificationId, eventId, notification.Message, notification.Created, emailAddress);
            return p;
        }
    }
}
