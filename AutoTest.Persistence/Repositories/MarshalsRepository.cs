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

        IQueryable<Marshal> IMarshalsRepository.GetByEmail(string emailAddress)
        {
            return _autoTestContext.Marshals!.Where(a => a.Email == emailAddress);
        }

        IQueryable<Marshal> IMarshalsRepository.GetByEventId(ulong eventId)
        {
            return _autoTestContext.Marshals!.Where(a => a.EventId == eventId);
        }

        Task<ulong> IMarshalsRepository.GetMashalIdByEmail(ulong eventId, string emailAddress, CancellationToken cancellationToken)
        {
            return _autoTestContext.Marshals!.Where(a => a.EventId == eventId && a.Email == emailAddress).Select(a => a.MarshalId).SingleAsync(cancellationToken);
        }

        Task<Marshal> IMarshalsRepository.GetById(ulong eventId, ulong marshalId, CancellationToken cancellationToken)
        {
            return _autoTestContext.Marshals!.SingleAsync(a => a.EventId == eventId && a.MarshalId == marshalId, cancellationToken);
        }
    }
}
