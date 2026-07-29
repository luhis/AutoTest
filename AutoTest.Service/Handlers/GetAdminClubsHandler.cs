using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetAdminClubsHandler(IClubsRepository clubsRepository) : IRequestHandler<GetAdminClubs, IEnumerable<ulong>>
{
    public ValueTask<IEnumerable<ulong>> Handle(GetAdminClubs request, CancellationToken cancellationToken) =>
        new(clubsRepository.GetClubIdsByEmail(request.EmailAddress, cancellationToken));
}
