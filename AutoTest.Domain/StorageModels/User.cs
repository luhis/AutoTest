namespace AutoTest.Domain.StorageModels
{
    public class User
    {
        public User(ulong userId, string givenName, string familyName, string msaLicense)
        {
            UserId = userId;
            GivenName = givenName;
            FamilyName = familyName;
            MsaLicense = msaLicense;
            //Vehicle = vehicle;
        }

        public ulong UserId { get; }

        public string GivenName { get; }

        public string FamilyName { get; }

        public string MsaLicense { get; }

        public Vehicle Vehicle { get; set; } = new Vehicle();
    }
}
