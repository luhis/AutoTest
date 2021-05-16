namespace AutoTest.Service.Messages
{
    using System.Collections.Generic;
    using AutoTest.Domain.StorageModels;
    using MediatR;

    public record GetMarshalEvents : IRequest<IEnumerable<ulong>>
    {
        public string EmailAddress { get; }

        public GetMarshalEvents(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
