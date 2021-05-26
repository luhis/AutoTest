namespace AutoTest.Web.Models
{
    public record PublicMarshalModel
    {
        public PublicMarshalModel(ulong marshalId, string givenName, string familyName, ulong eventId, string role)
        {
            MarshalId = marshalId;
            GivenName = givenName;
            FamilyName = familyName;
            EventId = eventId;
            Role = role;
        }
        public ulong MarshalId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }


        public ulong EventId { get; }
        public string Role { get; }
    }
}
