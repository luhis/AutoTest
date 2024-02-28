using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class ProfileRepository(AutoTestContext autoTestContext) : IProfileRepository
    {
        Task<Profile?> IProfileRepository.Get(string email, CancellationToken cancellationToken)
        {
            return autoTestContext.Users!.Where(a => a.EmailAddress == email).SingleOrDefaultAsync(cancellationToken);
        }

        async Task IProfileRepository.Upsert(Profile profile, CancellationToken cancellationToken)
        {
            await autoTestContext.Users!.Upsert(profile, a => a.EmailAddress == profile.EmailAddress, cancellationToken);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
