using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Web.Models.Display
{
    public class PublicEntrantModel(
        ulong entrantId,
        ushort driverNumber,
        string givenName,
        string familyName,
        string @class,
        ulong eventId,
        EntrantClub entrantClub,
        Vehicle vehicle,
        Payment? payment,
        bool isLady,
        EntrantStatus entrantStatus)
    {
        public ulong EntrantId { get; } = entrantId;

        public ushort DriverNumber { get; } = driverNumber;

        public string GivenName { get; } = givenName;

        public string FamilyName { get; } = familyName;

        public string Class { get; } = @class;

        public ulong EventId { get; } = eventId;

        public EntrantClub EntrantClub { get; } = entrantClub;

        public Vehicle Vehicle { get; } = vehicle;

        public Payment? Payment { get; } = payment;

        public bool IsLady { get; } = isLady;

        public EntrantStatus EntrantStatus { get; } = entrantStatus;
    }
}
