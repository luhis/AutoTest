using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public record MarshalSaveModel
    {

        [Required]
        public string GivenName { get; set; } = string.Empty;

        [Required]
        public string FamilyName { get; set; } = string.Empty;

        [Required]
        public uint RegistrationNumber { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public EmergencyContactSaveModel EmergencyContact { get; set; } = new EmergencyContactSaveModel();

        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
