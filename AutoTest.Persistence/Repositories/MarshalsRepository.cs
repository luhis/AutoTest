using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class MarshalsRepository : IMarshalsRepository
    {
        private readonly AutoTestContext _autoTestContext;

        public MarshalsRepository(AutoTestContext autoTestContext)
        {
            _autoTestContext = autoTestContext;
        }

        Task<IEnumerable<Marshal>> IMarshalsRepository.GetByEventId(ulong eventId, CancellationToken cancellationToken)
        {
            return _autoTestContext.Marshals!.Where(a => a.EventId == eventId).ToEnumerableAsync(cancellationToken);
        }

        Task<Marshal> IMarshalsRepository.GetById(ulong eventId, string emailAddress, CancellationToken cancellationToken)
        {
            return _autoTestContext.Marshals!.SingleAsync(a => a.EventId == eventId && a.Email == emailAddress, cancellationToken);
        }
    }
}
