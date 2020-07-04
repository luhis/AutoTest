using System;
using System.Collections.Generic;
using System.Linq;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Web.Models
{
    public class TestRunSaveModel
    {
        public ulong TestId { get; set; }

        public ulong TimeInMS { get; set; }

        public DateTime Created { get; set; }

        public ulong EntrantId { get; set; }

        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = Enumerable.Empty<PenaltySaveModel>();
    }

    public class PenaltySaveModel
    {
        public ulong PenaltyId { get; set; }

        public PenaltyEnum PenaltyType { get; set; }

        public uint InstanceCount { get; set; }
    }
}
