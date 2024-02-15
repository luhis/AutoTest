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

        private static EmergencyContact Map(EmergencyContactSaveModel emergencyContact)
        {
            return new EmergencyContact(emergencyContact.Name, emergencyContact.Phone);
        }

        private static MsaMembership Map(MsaMembershipSaveModel membership)
        {
            return new MsaMembership(membership.MsaLicenseType, membership.MsaLicense);
        }

        public static Payment Map(PaymentSaveModel payment, string currentUserEmail)
        {
            return new Payment(payment.PaidAt, payment.Method, payment.Timestamp, currentUserEmail);
        }

        public static AcceptDeclaration Map(AcceptDeclarationSaveModel acceptDeclaration)
        {
            return new AcceptDeclaration(acceptDeclaration.Email, acceptDeclaration.TimeStamp, acceptDeclaration.IsAccepted);
        }

        public static EntrantClub Map(EntrantClubSaveModel acceptDeclaration)
        {
            return new EntrantClub(acceptDeclaration.Club, acceptDeclaration.ClubNumber);
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
            var e = new Entrant(entrantId, entrant.DriverNumber, entrant.GivenName, entrant.FamilyName, email, entrant.Class, eventId, entrant.Age, entrant.IsLady);
            e.SetVehicle(Map(entrant.Vehicle));
            e.SetEmergencyContact(Map(entrant.EmergencyContact));
            e.SetMsaMembership(Map(entrant.MsaMembership));
            e.SetAcceptDeclaration(Map(entrant.AcceptDeclaration));
            e.SetEntrantClub(Map(entrant.EntrantClub));
            return e;
        }

        public static PublicEntrantModel Map(Entrant a) => new PublicEntrantModel(a.EntrantId, a.DriverNumber, a.GivenName, a.FamilyName, a.Class, a.EventId, new EntrantClub(a.EntrantClub.Club, ""), a.Vehicle, a.Payment, a.IsLady, a.EntrantStatus);

        public static Marshal Map(ulong marshalId, ulong eventId, MarshalSaveModel entrant, string email)
        {
            var e = new Marshal(marshalId, entrant.GivenName, entrant.FamilyName, email, eventId,
                entrant.RegistrationNumber, entrant.Role);
            e.SetEmergencyContact(Map(entrant.EmergencyContact));
            return e;
        }

        public static PublicMarshalModel Map(Marshal a) => new PublicMarshalModel(a.MarshalId, a.GivenName, a.FamilyName, a.EventId, a.Role);

        public static Profile Map(string emailAddress, ProfileSaveModel profile)
        {
            var p = new Profile(emailAddress, profile.GivenName, profile.FamilyName, profile.Age, profile.IsLady);
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
