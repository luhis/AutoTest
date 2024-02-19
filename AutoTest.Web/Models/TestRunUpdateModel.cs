using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public class TestRunUpdateModel
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int TimeInMS { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public ulong EntrantId { get; set; }

        [Required]
        public ulong MarshalId { get; set; }

        [Required]
        public IEnumerable<PenaltySaveModel> Penalties { get; set; } = [];
    }
}
