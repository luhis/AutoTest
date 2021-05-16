using System;

namespace AutoTest.Domain.StorageModels
{
    public class AcceptDeclaration
    {
        public AcceptDeclaration()
        {

        }
        public AcceptDeclaration(string email, DateTime timestamp)
        {
            Email = email;
            Timestamp = timestamp;
        }
        public string Email { get; } = string.Empty;

        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
