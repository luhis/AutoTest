using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AutoTest.Web.Models.Save
{
    public class TestRunSaveModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TimeInMS { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public ulong EntrantId { get; set; }

        [Required]
        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = Enumerable.Empty<PenaltySaveModel>();
    }
}
