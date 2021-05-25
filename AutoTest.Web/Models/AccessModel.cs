using System.Collections.Generic;

namespace AutoTest.Web.Models
{
    public record AccessModel
    {
        public AccessModel(bool isRootAdmin, bool isLoggedIn, bool canViewClubs, bool canViewProfile, IEnumerable<ulong> adminClubs, IEnumerable<ulong> marshalEvents, IEnumerable<ulong> editableEntrants, IEnumerable<ulong> editableMarshals)
        {
            IsRootAdmin = isRootAdmin;
            this.CanViewClubs = canViewClubs;
            CanViewProfile = canViewProfile;
            IsLoggedIn = isLoggedIn;
            AdminClubs = adminClubs;
            MarshalEvents = marshalEvents;
            EditableEntrants = editableEntrants;
            EditableMarshals = editableMarshals;
        }

        public bool IsRootAdmin { get; }
        public bool IsLoggedIn { get; }
        public bool CanViewClubs { get; }
        public bool CanViewProfile { get; }

        public IEnumerable<ulong> AdminClubs { get; }

        public IEnumerable<ulong> MarshalEvents { get; }

        public IEnumerable<ulong> EditableEntrants { get; }

        public IEnumerable<ulong> EditableMarshals { get; }
    }
}
