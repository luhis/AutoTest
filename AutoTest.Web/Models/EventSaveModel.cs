using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTest.Web.Models
{
    public class EventSaveModel
    {
        public ulong ClubId { get; set; }

        public string Location { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public int TestCount { get; set; }

        public int MaxAttemptsPerTest { get; set; }

        public IEnumerable<AuthorisationEmailSaveModel> MarshalEmails { get; set; } = Enumerable.Empty<AuthorisationEmailSaveModel>();
    }
}
