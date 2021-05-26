using AutoTest.Domain.StorageModels;

namespace AutoTest.Web.Models
{
    public class PublicEntrantModel
    {
        public PublicEntrantModel(ulong entrantId, ushort driverNumber, string givenName, string familyName, string @class, ulong eventId, string club, Vehicle vehicle)
        {
            EntrantId = entrantId;
            DriverNumber = driverNumber;
            GivenName = givenName;
            FamilyName = familyName;
            Class = @class;
            EventId = eventId;
            Club = club;
            Vehicle = vehicle;
        }

        public ulong EntrantId { get; }

        public ushort DriverNumber { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string Class { get; }

        public ulong EventId { get; }

        public string Club { get; }

        public Vehicle Vehicle { get; }
    }
}
