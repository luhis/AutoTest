using System;

namespace AutoTest.Web.Models
{
    public class AcceptDeclarationSaveModel
    {
        public string Email { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; }
        public bool IsAccepted { get; set; }
    }
}
