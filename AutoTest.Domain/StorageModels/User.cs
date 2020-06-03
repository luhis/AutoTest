namespace AutoTest.Domain.StorageModels
{
    public class User
    {
        public User(ulong userId, string givenName, string familyName)
        {
            UserId = userId;
            GivenName = givenName;
            FamilyName = familyName;
        }

        public ulong UserId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }
    }
}