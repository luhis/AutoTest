using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save
{
    public class AuthorisationEmailSaveModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
