using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetProfileHandler(IProfileRepository profileRepository) : IRequestHandler<GetProfile, Profile>
{
    public async ValueTask<Profile> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        var found = await profileRepository.Get(request.EmailAddress, cancellationToken);
        return found is null ? new Profile(request.EmailAddress, "", "", Age.Senior, false) : found;
    }
}
