using System;

namespace AutoTest.Domain.StorageModels;

public record AcceptDeclaration(string Email, DateTime Timestamp, bool IsAccepted);
