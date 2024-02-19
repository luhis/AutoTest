using System;
using System.ComponentModel.DataAnnotations;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models.Save
{
    public class PaymentSaveModel
    {
        [Required]
        public DateTime PaidAt { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}
