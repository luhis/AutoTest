using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Entrant
    {
        public Entrant(ulong entrantId, ushort driverNumber, string givenName, string familyName, string email,
        string @class, ulong eventId, Age age, bool isLady)
        {
            EntrantId = entrantId;
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            Class = @class;
            EventId = eventId;
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

        public string Email { get; }

        public Age Age { get; }

        public bool IsLady { get; }

        public EntrantClub EntrantClub { get; private set; } = new EntrantClub();

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public MsaMembership MsaMembership { get; private set; } = new MsaMembership();

        public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

        public Payment? Payment { get; private set; }

        public ulong? DoubleDrivenWith { get; private set; }

        public EntrantStatus EntrantStatus { get; private set; }

        public void SetVehicle(Vehicle vehicle) => Vehicle = vehicle;

        public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;

        public void SetMsaMembership(MsaMembership msaMembership) => MsaMembership = msaMembership;

        public void SetAcceptDeclaration(AcceptDeclaration acceptDeclaration) => AcceptDeclaration = acceptDeclaration;

        public void SetPayment(Payment? payment) => Payment = payment;

        public void SetEntrantStatus(EntrantStatus newStatus)
        {
            this.EntrantStatus = newStatus;
        }

        public void SetEntrantClub(EntrantClub newStatus)
        {
            this.EntrantClub = newStatus;
        }

        public void SetDoubleDrivenWith(ulong entrantId)
        {
            this.DoubleDrivenWith = entrantId;
        }
    }
}
