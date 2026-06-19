namespace AutoTest.Domain.StorageModels;

public class Marshal(ulong marshalId, string givenName, string familyName, string email, ulong eventId, uint registrationNumber, string role)
{
    public Marshal() : this(0, string.Empty, string.Empty, string.Empty, 0, 0, string.Empty) { }
    public ulong MarshalId { get; set; } = marshalId;

    public string GivenName { get; } = givenName;

    public string FamilyName { get; } = familyName;


    public ulong EventId { get; } = eventId;

    public uint RegistrationNumber { get; } = registrationNumber;
    public string Role { get; } = role;

    public string Email { get; } = email;
    public AcceptDeclaration AcceptDeclaration { get; private set; } = new AcceptDeclaration();

    public EmergencyContact EmergencyContact { get; private set; } = new EmergencyContact();

    public void SetEmergencyContact(EmergencyContact emergencyContact) => EmergencyContact = emergencyContact;

    public void SetAcceptDeclaration(AcceptDeclaration acceptDeclaration) => AcceptDeclaration = acceptDeclaration;
}
