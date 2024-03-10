using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetProfileHandler(IProfileRepository profileRepository) : IRequestHandler<GetProfile, Profile>
    {
        async Task<Profile> IRequestHandler<GetProfile, Profile>.Handle(GetProfile request, CancellationToken cancellationToken)
        {
            var found = await profileRepository.Get(request.EmailAddress, cancellationToken);
            return found == null ? new Profile(request.EmailAddress, "", "", Age.Senior, false) : found;
        }
    }
}
