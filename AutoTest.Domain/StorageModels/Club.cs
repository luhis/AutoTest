namespace AutoTest.Domain.StorageModels
{
    public class Club
    {
        public Club(ulong clubId, string clubName, string clubPaymentAddress, string website)
        {
            ClubId = clubId;
            ClubName = clubName;
            ClubPaymentAddress = clubPaymentAddress;
            Website = website;
        }

        public ulong ClubId { get; }

        public string ClubName { get; }

        public string ClubPaymentAddress { get; }

        public string Website { get; }
    }
}
