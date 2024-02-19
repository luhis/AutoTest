using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Display;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapMarshal
    {
        public static Marshal Map(ulong marshalId, ulong eventId, MarshalSaveModel entrant, string email)
        {
            var e = new Marshal(marshalId, entrant.GivenName, entrant.FamilyName, email, eventId,
                entrant.RegistrationNumber, entrant.Role);
            e.SetEmergencyContact(MapEmergencyContact.Map(entrant.EmergencyContact));
            return e;
        }

        public static PublicMarshalModel Map(Marshal a) => new PublicMarshalModel(a.MarshalId, a.GivenName, a.FamilyName, a.EventId, a.Role);

    }
}
