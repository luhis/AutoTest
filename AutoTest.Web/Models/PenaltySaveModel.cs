using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Web.Models
{
    public class PenaltySaveModel
    {
        public PenaltyEnum PenaltyType { get; set; }

        public int InstanceCount { get; set; }
    }
}
