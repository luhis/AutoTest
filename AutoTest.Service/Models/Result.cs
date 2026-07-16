using System.Collections.Generic;

namespace AutoTest.Service.Models;

public record Result(string Class, IEnumerable<EntrantTimes> EntrantTimes);
