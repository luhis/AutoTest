using System.Linq;
using AutoTest.Domain.StorageModels;
using AutoTest.Web.Models.Save;

namespace AutoTest.Web.Mapping
{
    public static class MapProfile
    {

        private static ClubMembership Map(ClubMembershipSaveModel membership)
        {
            return new ClubMembership(membership.ClubName, membership.MembershipNumber, membership.Expiry);
        }

        public static Profile Map(string emailAddress, ProfileSaveModel profile)
        {
            var p = new Profile(emailAddress, profile.GivenName, profile.FamilyName, profile.Age, profile.IsLady);
            p.SetVehicle(MapVehicle.Map(profile.Vehicle));
            p.SetEmergencyContact(MapEmergencyContact.Map(profile.EmergencyContact));
            p.SetClubMemberships(profile.ClubMemberships.Select(Map).ToArray());
            p.SetMsaMembership(MapMsaMembership.Map(profile.MsaMembership));
            return p;
        }
    }
}
