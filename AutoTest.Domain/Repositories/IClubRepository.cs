using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IClubRepository
    {
        Task<Club?> GetById(ulong clubId, CancellationToken cancellationToken);

        Task Delete(ulong clubId, CancellationToken cancellationToken);

        Task<IEnumerable<Club>> GetAll(CancellationToken cancellationToken);

        Task Upsert(Club club, CancellationToken cancellationToken);
    }
}
