using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Payment
    {
        public Payment()
        {
        }

        public Payment(DateTime paidAt, PaymentMethod method, DateTime timestamp)
        {
            PaidAt = paidAt;
            Method = method;
            Timestamp = timestamp;
        }

        public DateTime PaidAt { get; }
        public PaymentMethod Method { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

    }
}
