using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class SaveProfileHandler(IProfileRepository profileRepository) : IRequestHandler<SaveProfile, string>
{
    public async ValueTask<string> Handle(SaveProfile request, CancellationToken cancellationToken)
    {
        await profileRepository.Upsert(request.Profile, cancellationToken);
        return request.Profile.EmailAddress;
    }
}
