using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models.Save;

public record TestRunSaveModel
{
    [Required]
    [Range(1, int.MaxValue)]
    public int TimeInMS { get; init; }

    [Required]
    public DateTime Created { get; init; }

    [Required]
    public ulong EntrantId { get; init; }

    [Required]
    public IEnumerable<PenaltySaveModel> Penalties { get; init; } = [];
}
