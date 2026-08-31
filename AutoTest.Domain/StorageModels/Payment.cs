using System;
using AutoTest.Domain.Enums;

namespace AutoTest.Domain.StorageModels;

public record Payment(DateTime PaidAt, PaymentMethod Method, DateTime Timestamp, string CreatedBy);
