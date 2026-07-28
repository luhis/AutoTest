using System;
using System.Collections.Generic;

namespace AutoTest.Web;

public record RootAdminEmails(IReadOnlySet<string> Emails)
{
    public static RootAdminEmails FromConfig(IEnumerable<string> emails) =>
        new(new HashSet<string>(emails, StringComparer.OrdinalIgnoreCase));

    public bool Contains(string email) => Emails.Contains(email);
}
