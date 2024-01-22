using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Web.Models
{
    public class PublicEntrantModel(
        ulong entrantId,
        ushort driverNumber,
        string givenName,
        string familyName,
        EventType eventType,
        string @class,
        ulong eventId,
        string club,
        Vehicle vehicle,
        Payment? payment)
    {
        public ulong EntrantId { get; } = entrantId;

        public ushort DriverNumber { get; } = driverNumber;

        public string GivenName { get; } = givenName;

        public string FamilyName { get; } = familyName;

        public EventType EventType { get; } = eventType;

        public string Class { get; } = @class;

        public ulong EventId { get; } = eventId;

        public string Club { get; } = club;

        public Vehicle Vehicle { get; } = vehicle;

        public Payment? Payment { get; } = payment;
    }
}
