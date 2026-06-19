using System.Collections.Generic;

namespace AutoTest.Web.Models.Display;

public record AccessModel(bool IsRootAdmin, bool IsLoggedIn, bool CanViewClubs, bool CanViewProfile, IEnumerable<ulong> AdminClubs, IEnumerable<ulong> MarshalEvents, IEnumerable<ulong> EditableEntrants, IEnumerable<ulong> EditableMarshals);
