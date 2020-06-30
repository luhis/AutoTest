namespace AutoTest.Web.Models
{
    public class EntrantSaveModel
    {
        public string GivenName { get; set; } = string.Empty;

        public string FamilyName { get; set; } = string.Empty;

        public string Registration { get; set; } = string.Empty;

        public string Class { get; set; } = string.Empty;

        public ulong EventId { get; set; }

        public bool IsPaid { get; set; }
    }
}
