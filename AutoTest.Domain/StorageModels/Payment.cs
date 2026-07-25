using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels;

public record Payment(DateTime PaidAt = default, PaymentMethod Method = default, DateTime Timestamp = default, string CreatedBy = "");
