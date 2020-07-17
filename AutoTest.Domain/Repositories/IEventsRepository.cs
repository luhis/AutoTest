using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface IEventsRepository
    {
        Task<Event?> GetById(ulong eventId);
    }
}
