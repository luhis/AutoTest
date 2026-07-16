using System.Collections.Generic;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Service.Models;

public record TestTime(int Ordinal, IEnumerable<TestRun> TestRuns);
