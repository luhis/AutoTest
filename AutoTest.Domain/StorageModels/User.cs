namespace AutoTest.Domain.StorageModels
{
    public class User
    {
        public User(ulong userId, string givenName, string familyName, string msaLicense)
        {
            UserId = userId;
            GivenName = givenName;
            FamilyName = familyName;
            this.MsaLicense = msaLicense;
        }

        public ulong UserId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string MsaLicense { get; }
    }
}