using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetProfile : IRequest<Profile>
    {
        public GetProfile(string emailAddress)
        {
            this.EmailAddress = emailAddress;
        }

        public string EmailAddress { get; }
    }
}
