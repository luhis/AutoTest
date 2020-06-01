namespace AutoTest.Service.Handlers
{
    using System.Collections.Generic;
    using AutoTest.Service.Messages;
    using AutoTest.Domain.StorageModels;
    using MediatR;
    using System.Threading.Tasks;
    using System.Threading;

    public class GetClubsHandler : IRequestHandler<GetClubs, IEnumerable<Club>>
    {
        Task<IEnumerable<Club>> IRequestHandler<GetClubs, IEnumerable<Club>>.Handle(GetClubs request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
