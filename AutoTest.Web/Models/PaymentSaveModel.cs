using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Web.Models
{
    public class PaymentSaveModel
    {
        public DateTime PaidAt { get; set; }
        public PaymentMethod Method { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
