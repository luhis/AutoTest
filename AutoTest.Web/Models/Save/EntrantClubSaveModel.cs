using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save
{
    public class EntrantClubSaveModel
    {
        [Required]
        public string Club { get; set; } = string.Empty;

        [Required]
        public string ClubNumber { get; set; } = string.Empty;
    }
}
