namespace AutoTest.Domain.StorageModels
{
    public class AdminEmail
    {
        public AdminEmail(ulong adminEmailId, ulong clubId, string email)
        {
            this.AdminEmailId = adminEmailId;
            this.ClubId = clubId;
            this.Email = email;
        }

        public  ulong AdminEmailId { get; }

        public  ulong ClubId { get; }

        public string Email { get; }
    }
}