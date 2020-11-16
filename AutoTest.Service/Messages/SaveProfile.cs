using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SaveProfile : IRequest<string>
    {
        public SaveProfile(string emailAddress, Profile profile)
        {
            this.Profile = profile;
            EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }

        public Profile Profile { get; }
    }
}
