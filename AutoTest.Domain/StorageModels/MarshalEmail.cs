namespace AutoTest.Domain.StorageModels
{
    public class MarshalEmail
    {
        public MarshalEmail(ulong marshalEmailId, ulong clubId, string email)
        {
            this.MarshalEmailId = marshalEmailId;
            this.ClubId = clubId;
            this.Email = email;
        }

        public ulong MarshalEmailId { get; }

        public ulong ClubId { get; }

        public string Email { get; }
    }
}