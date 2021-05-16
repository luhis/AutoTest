using System.Collections.Generic;

namespace AutoTest.Web.Models
{
    public record AccessModel
    {
        public AccessModel(bool isLoggedIn, bool canViewClubs, bool canViewProfile, IEnumerable<ulong> adminClubs, IEnumerable<ulong> marshalEvents)
        {
            this.CanViewClubs = canViewClubs;
            CanViewProfile = canViewProfile;
            IsLoggedIn = isLoggedIn;
            AdminClubs = adminClubs;
            MarshalEvents = marshalEvents;
        }

        public bool IsLoggedIn { get; }
        public bool CanViewClubs { get; }
        public bool CanViewProfile { get; }

        public IEnumerable<ulong> AdminClubs { get; }

        public IEnumerable<ulong> MarshalEvents { get; }
    }
}
