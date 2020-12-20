using System;

namespace AutoTest.Web.Models
{
    public class NotificationSaveModel
    {

        public string Message { get; set; } = string.Empty;

        public DateTime Created { get; set; }
    }
}
