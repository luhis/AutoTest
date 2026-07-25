using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Persistence.Repositories;

public class ProfileRepository(AutoTestContext autoTestContext) : IProfileRepository
{
    Task<Profile?> IProfileRepository.Get(string email, CancellationToken cancellationToken)
    {
        return autoTestContext.Users.FindAsync([email], cancellationToken).AsTask();
    }

    async Task IProfileRepository.Upsert(Profile profile, CancellationToken cancellationToken)
    {
        await autoTestContext.Users.Upsert(profile, a => a.EmailAddress == profile.EmailAddress, cancellationToken);
        await autoTestContext.SaveChangesAsync(cancellationToken);
    }
}
