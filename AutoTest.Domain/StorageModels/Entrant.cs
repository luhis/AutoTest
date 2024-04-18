using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Entrant(ulong entrantId, ushort driverNumber, string givenName, string familyName, string email,
    string @class, ulong eventId, Age age, bool isLady, ulong? doubleDrivenWith)
    {
        public ulong EntrantId { get; } = entrantId;

        public ushort DriverNumber { get; } = driverNumber;

        public string GivenName { get; } = givenName;

        public string FamilyName { get; } = familyName;

        public string Class { get; } = @class;

        public ulong EventId { get; } = eventId;

        public string Email { get; } = email;

        public Age Age { get; } = age;

        public bool IsLady { get; } = isLady;

        public EntrantClub EntrantClub { get; private set; } = new EntrantClub();

        public Vehicle Vehicle { get; private set; } = new Vehicle();

        public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

        public MsaMembership MsaMembership { get; private set; } = new MsaMembership();

        public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

        public Payment? Payment { get; private set; }

        public ulong? DoubleDrivenWith { get; private set; } = doubleDrivenWith;

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
    }
}
