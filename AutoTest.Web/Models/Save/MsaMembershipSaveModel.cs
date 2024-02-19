using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save
{
    public class MsaMembershipSaveModel
    {
        [Required]
        public uint MsaLicense { get; set; }

        [Required]
        public string MsaLicenseType { get; set; } = string.Empty;
    }
}
