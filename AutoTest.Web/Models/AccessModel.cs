namespace AutoTest.Web.Models
{
    public class AccessModel
    {
        public AccessModel(bool isLoggedIn, bool canViewClubs, bool canViewProfile)
        {
            this.CanViewClubs = canViewClubs;
            CanViewProfile = canViewProfile;
            IsLoggedIn = isLoggedIn;
        }

        public bool IsLoggedIn { get; }
        public bool CanViewClubs { get; }
        public bool CanViewProfile { get; }
    }
}
