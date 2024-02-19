using System;
using System.ComponentModel.DataAnnotations;

namespace AutoTest.Web.Models
{
    public class NotificationSaveModel
    {

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }
    }
}
