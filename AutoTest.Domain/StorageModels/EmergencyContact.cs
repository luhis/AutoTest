namespace AutoTest.Domain.StorageModels
{
    public class EmergencyContact
    {
        public EmergencyContact(ulong emergencyContactId, string name, string phone)
        {
            this.EmergencyContactId = emergencyContactId;
            this.Name = name;
            this.Phone = phone;
        }

        public ulong EmergencyContactId { get; }

        public string Name { get; }

        public string Phone { get; }
    }
}