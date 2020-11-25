using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Web.Models
{
    public class TestRunSaveModel
    {
        public int TimeInMS { get; set; }

        public DateTime Created { get; set; }

        public ulong EntrantId { get; set; }

        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = Enumerable.Empty<PenaltySaveModel>();
    }
}
