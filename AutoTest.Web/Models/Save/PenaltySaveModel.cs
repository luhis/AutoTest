using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models.Save
{
    public class PenaltySaveModel
    {
        [Required]
        public PenaltyEnum PenaltyType { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int InstanceCount { get; set; }
    }
}
