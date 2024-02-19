using System;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public class AcceptDeclarationSaveModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public DateTime TimeStamp { get; set; }
        [Required]
        public bool IsAccepted { get; set; }
    }
}
