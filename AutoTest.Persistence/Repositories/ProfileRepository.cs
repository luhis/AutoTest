using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public ProfileRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        Task<Profile?> IProfileRepository.Get(string email, CancellationToken cancellationToken)
        {
            return this._autoTestContext.Users!.Where(a => a.EmailAddress == email).SingleOrDefaultAsync(cancellationToken);
        }

        async Task IProfileRepository.Upsert(Profile profile, CancellationToken cancellationToken)
        {
            await _autoTestContext.Users!.Upsert(profile, a => a.EmailAddress == profile.EmailAddress, cancellationToken);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
