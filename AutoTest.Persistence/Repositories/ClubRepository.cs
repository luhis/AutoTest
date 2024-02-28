using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class ClubRepository(AutoTestContext autoTestContext) : IClubsRepository
    {
        async Task<Club?> IClubsRepository.GetById(ulong clubId, CancellationToken cancellationToken)
        {
            return await autoTestContext.Clubs!.Where(a => a.ClubId == clubId).SingleOrDefaultAsync(cancellationToken);
        }

        async Task IClubsRepository.Delete(ulong clubId, CancellationToken cancellationToken)
        {
            var found = await autoTestContext.Clubs!.SingleAsync(a => a.ClubId == clubId, cancellationToken);
            autoTestContext.Clubs!.Remove(found);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task<IEnumerable<Club>> IClubsRepository.GetAll(CancellationToken cancellationToken) =>
            autoTestContext.Clubs!.OrderBy(a => a.ClubName).ToEnumerableAsync(cancellationToken);

        async Task IClubsRepository.Upsert(Club club, CancellationToken cancellationToken)
        {
            await autoTestContext.Clubs!.Upsert(club, a => a.ClubId == club.ClubId, cancellationToken);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
