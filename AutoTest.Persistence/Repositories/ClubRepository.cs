using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public ClubRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Club?> IClubRepository.GetById(ulong clubId, CancellationToken cancellationToken)
        {
            return await _autoTestContext.Clubs!.Where(a => a.ClubId == clubId).SingleOrDefaultAsync(cancellationToken);
        }

        async Task IClubRepository.Delete(ulong clubId, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.Clubs.SingleAsync(a => a.ClubId == clubId, cancellationToken);
            this._autoTestContext.Clubs.ThrowIfNull().Remove(found);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task<IEnumerable<Club>> IClubRepository.GetAll(CancellationToken cancellationToken)
        {
            return this._autoTestContext.Clubs!.OrderBy(a => a.ClubName).ToEnumerableAsync(cancellationToken);
        }

        async Task IClubRepository.Upsert(Club club, CancellationToken cancellationToken)
        {
            await this._autoTestContext.Clubs.ThrowIfNull().Upsert(club, a => a.ClubId == club.ClubId, cancellationToken);
            await this._autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
