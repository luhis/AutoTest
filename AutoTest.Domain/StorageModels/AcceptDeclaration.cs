using System;

namespace AutoTest.Domain.StorageModels;

public record AcceptDeclaration(string Email = "", DateTime Timestamp = default, bool IsAccepted = false);
