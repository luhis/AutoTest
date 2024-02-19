using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public class EmergencyContactSaveModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
