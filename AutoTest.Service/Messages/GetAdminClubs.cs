namespace AutoTest.Service.Messages
{
    using System.Collections.Generic;
    using MediatR;

    public class GetAdminClubs : IRequest<IEnumerable<ulong>>
    {
        public string EmailAddress { get; }

        public GetAdminClubs(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
