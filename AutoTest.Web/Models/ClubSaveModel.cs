using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public class ClubSaveModel
    {
        [Required]
        public string ClubName { get; set; } = string.Empty;

        [Required]
        public string ClubPaymentAddress { get; set; } = string.Empty;

        [Required]
        public string Website { get; set; } = string.Empty;

        [Required]
        public IEnumerable<AuthorisationEmailSaveModel> AdminEmails { get; set; } = [];
    }
}
