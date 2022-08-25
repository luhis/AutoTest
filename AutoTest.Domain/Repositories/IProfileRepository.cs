using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IProfileRepository
    {
        Task Upsert(Profile profile, CancellationToken cancellationToken);
        Task<Profile?> Get(string email, CancellationToken cancellationToken);
    }
}
