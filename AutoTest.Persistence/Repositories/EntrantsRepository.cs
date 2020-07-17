using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class EntrantsRepository : IEntrantsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public EntrantsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        async Task<Entrant?> IEntrantsRepository.GetById(ulong entrantId)
        {
            return await _autoTestContext.Entrants.Where(a => a.EntrantId == entrantId).SingleOrDefaultAsync();
        }
    }
}
