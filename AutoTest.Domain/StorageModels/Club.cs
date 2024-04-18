using System.Collections.Generic;

namespace AutoTest.Domain.StorageModels
{
    public class Club(ulong clubId, string clubName, string clubPaymentAddress, string website)
    {
        public ulong ClubId { get; } = clubId;

        public string ClubName { get; } = clubName;

        public string ClubPaymentAddress { get; } = clubPaymentAddress;

        public string Website { get; } = website;

        public ICollection<AuthorisationEmail> AdminEmails { get; private set; } = new List<AuthorisationEmail>();

        public void SetAdminEmails(ICollection<AuthorisationEmail> emails) => AdminEmails = emails;
    }
}
