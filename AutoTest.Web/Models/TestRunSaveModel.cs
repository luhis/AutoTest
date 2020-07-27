using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Web.Models
{
    public class TestRunSaveModel
    {
        public ulong EventId { get; set; }

        public int Ordinal { get; set; }

        public int TimeInMS { get; set; }

        public DateTime Created { get; set; }

        public ulong EntrantId { get; set; }

        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = Enumerable.Empty<PenaltySaveModel>();
    }
}
