using System;

namespace AutoTest.Domain.StorageModels
{
    public class AcceptDeclaration
    {
        public AcceptDeclaration()
        {

        }
        public AcceptDeclaration(string email, DateTime timestamp, bool isAccepted)
        {
            Email = email;
            Timestamp = timestamp;
            IsAccepted = isAccepted;
        }

        public bool IsAccepted { get; } = false;

        public string Email { get; } = string.Empty;

        public DateTime Timestamp { get; }
    }
}
