using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save
{
    public class EmergencyContactSaveModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
    }
}
