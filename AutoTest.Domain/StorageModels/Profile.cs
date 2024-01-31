using System.Collections.Generic;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Profile
    {

        public Profile(string emailAddress, string givenName, string familyName, Age age, bool isLady)
        {
            EmailAddress = emailAddress;
            GivenName = givenName;
            FamilyName = familyName;
            Age = age;
            IsLady = isLady;
        }

        public string EmailAddress { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public Age Age { get; }

        public bool IsLady { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();
        public MsaMembership MsaMembership { get; private set; } = new MsaMembership();

        public ICollection<ClubMembership> ClubMemberships { get; private set; } = new List<ClubMembership>();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;

        public void SetClubMemberships(ICollection<ClubMembership> clubMemberships) => ClubMemberships = clubMemberships;

        public void SetMsaMembership(MsaMembership msaMembership) => MsaMembership = msaMembership;
    }
}
