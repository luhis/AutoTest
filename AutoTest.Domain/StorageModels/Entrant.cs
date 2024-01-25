using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {
        public Entrant(ulong entrantId, ushort driverNumber, string givenName, string familyName, string email,
        EventType eventType, string @class, ulong eventId, string club, uint clubNumber, Age age, bool isLady)
        {
            EntrantId = entrantId;
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            EventType = eventType;
            Class = @class;
            EventId = eventId;
            Club = club;
            ClubNumber = clubNumber;
            Age = age;
            DriverNumber = driverNumber;
            IsLady = isLady;
        }

        public ulong EntrantId { get; }

        public ushort DriverNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public string Club { get; }

        public uint ClubNumber { get; } // make a club obj?

        public string Email { get; }

        public Age Age { get; }

        public bool IsLady { get; }

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public MsaMembership MsaMembership { get; private set; } = new MsaMembership();

        public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

        public Payment? Payment { get; private set; } = null;

        public EntrantStatus EntrantStatus { get; private set; }
        public EventType EventType { get; private set; }

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;

        public void SetMsaMembership(MsaMembership msaMembership) => MsaMembership = msaMembership;

        public void SetAcceptDeclaration(AcceptDeclaration acceptDeclaration) => AcceptDeclaration = acceptDeclaration;

        public void SetPayment(Payment? payment) => Payment = payment;

        public void SetEntrantStatus(EntrantStatus newStatus)
        {
            this.EntrantStatus = newStatus;
        }
    }
}
