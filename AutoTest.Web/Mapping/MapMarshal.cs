using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Display;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping;

public static class MapMarshal
{
    public static Marshal Map(ulong marshalId, ulong eventId, MarshalSaveModel marshal, string email)
    {
        var e = new Marshal(marshalId, marshal.GivenName, marshal.FamilyName, email, eventId,
            marshal.RegistrationNumber, marshal.Role);
        e.SetEmergencyContact(MapEmergencyContact.Map(marshal.EmergencyContact));
        return e;
    }

    public static PublicMarshalModel Map(Marshal a) => new PublicMarshalModel(a.MarshalId, a.GivenName, a.FamilyName, a.EventId, a.Role);
}
