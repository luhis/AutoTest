namespace AutoTest.Domain.StorageModels
{
    public class EmergencyContact
    {
        public EmergencyContact()
        {

        }

        public EmergencyContact(string name, string phone)
        {
            this.Name = name;
            this.Phone = phone;
        }

        public string Name { get; } = string.Empty;

        public string Phone { get; } = string.Empty;
    }
}
