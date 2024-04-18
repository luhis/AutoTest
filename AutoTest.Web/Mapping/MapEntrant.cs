using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Display;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapEntrant
    {
        public static Entrant Map(ulong entrantId, ulong eventId, EntrantSaveModel entrant, string email)
        {
            var e = new Entrant(entrantId, entrant.DriverNumber, entrant.GivenName, entrant.FamilyName, email, entrant.Class, eventId, entrant.Age, entrant.IsLady, entrant.DoubleDrivenWith);
            e.SetVehicle(MapVehicle.Map(entrant.Vehicle));
            e.SetEmergencyContact(MapEmergencyContact.Map(entrant.EmergencyContact));
            e.SetMsaMembership(MapMsaMembership.Map(entrant.MsaMembership));
            e.SetAcceptDeclaration(Map(entrant.AcceptDeclaration));
            e.SetEntrantClub(Map(entrant.EntrantClub));
            return e;
        }

        private static AcceptDeclaration Map(AcceptDeclarationSaveModel acceptDeclaration)
        {
            return new AcceptDeclaration(acceptDeclaration.Email, acceptDeclaration.TimeStamp, acceptDeclaration.IsAccepted);
        }

        private static EntrantClub Map(EntrantClubSaveModel acceptDeclaration)
        {
            return new EntrantClub(acceptDeclaration.Club, acceptDeclaration.ClubNumber);
        }

        public static PublicEntrantModel Map(Entrant a) => new PublicEntrantModel(a.EntrantId, a.DriverNumber, a.GivenName, a.FamilyName, a.Class, a.EventId, new EntrantClub(a.EntrantClub.Club, ""), a.Vehicle, a.Payment, a.IsLady, a.EntrantStatus);

    }
}
