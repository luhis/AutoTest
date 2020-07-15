using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Web.Models
{
    public class ClubSaveModel
    {
        public string ClubName { get; set; } = string.Empty;

        public string ClubPaymentAddress { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public IEnumerable<AuthorisationEmailSaveModel> AdminEmails { get; set; } = Enumerable.Empty<AuthorisationEmailSaveModel>();
    }
}
