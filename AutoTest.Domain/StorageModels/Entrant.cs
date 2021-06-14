using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {
        public Entrant(ulong entrantId, ushort driverNumber, string givenName, string familyName, string email, string @class, ulong eventId, bool isPaid, string club, uint clubNumber, Age age)
        {
            EntrantId = entrantId;
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            Class = @class;
            EventId = eventId;
            IsPaid = isPaid;
            Club = club;
            ClubNumber = clubNumber;
            Age = age;
            DriverNumber = driverNumber;
        }

        public ulong EntrantId { get; }

        public ushort DriverNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public bool IsPaid { get; }

        public string Club { get; }

        public uint ClubNumber { get; }

        public string Email { get; }

        public Age Age { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public MsaMembership MsaMembership { get; private set; } = new MsaMembership();

        public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;

        public void SetMsaMembership(MsaMembership msaMembership) => MsaMembership = msaMembership;

        public void SetAcceptDeclaration(AcceptDeclaration acceptDeclaration) => AcceptDeclaration = acceptDeclaration;
    }
}
