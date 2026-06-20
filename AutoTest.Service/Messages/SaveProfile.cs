using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class SaveProfile : IRequest<string>
{
    public SaveProfile(string emailAddress, Profile profile)
    {
        Profile = profile;
        EmailAddress = emailAddress;
    }

    public string EmailAddress { get; }

    public Profile Profile { get; }
}
