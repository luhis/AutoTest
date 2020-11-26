using System.Collections.Generic;

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

        public ICollection<AuthorisationEmail> AdminEmails { get; private set; } = new List<AuthorisationEmail>();

        public void SetAdminEmails(ICollection<AuthorisationEmail> emails) => AdminEmails = emails;
    }
}
