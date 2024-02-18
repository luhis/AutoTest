using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models;

namespace AutoTest.Web.Mapping
{
    public static class MapEmergencyContact
    {
        public static EmergencyContact Map(EmergencyContactSaveModel emergencyContact)
        {
            return new EmergencyContact(emergencyContact.Name, emergencyContact.Phone);
        }
    }
}
