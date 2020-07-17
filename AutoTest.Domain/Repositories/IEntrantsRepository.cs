using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IEntrantsRepository
    {
        Task<Entrant?> GetById(ulong entrantId);
    }
}
