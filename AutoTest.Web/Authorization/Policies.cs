namespace AutoTest.Web.Authorization
{
    public static class Policies
    {
        public const string Admin = nameof(Admin);
        public const string ClubAdmin = nameof(ClubAdmin);
        public const string ClubAdminOrSelf = nameof(ClubAdminOrSelf);
        public const string Marshal = nameof(Marshal);
    }
}
