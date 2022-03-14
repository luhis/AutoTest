using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels
{
    public class Payment
    {
        public Payment()
        {
        }

        public Payment(DateTime payedAt, PaymentMethod method, DateTime timestamp)
        {
            PayedAt = payedAt;
            Method = method;
            Timestamp = timestamp;
        }

        public DateTime PayedAt { get; }
        public PaymentMethod Method { get; }
        public DateTime Timestamp { get; } = DateTime.UtcNow;

    }
}
