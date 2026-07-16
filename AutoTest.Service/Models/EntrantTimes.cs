using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models;

public record EntrantTimes(Entrant Entrant, int TotalTime, IEnumerable<TestTime> Times, int Position, int ClassPosition);
