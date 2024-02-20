using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models.Save
{
    public class VehicleSaveModel
    {
        [Required]
        public string Make { get; set; } = string.Empty;

        [Required]
        public string Model { get; set; } = string.Empty;

        [Required]
        public string Registration { get; set; } = string.Empty;


        [Required]
        [Range(1, int.MaxValue)]
        public int Displacement { get; set; }

        [Required]
        public Induction Induction { get; set; }
    }
}
