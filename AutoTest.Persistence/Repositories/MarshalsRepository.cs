using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Persistence.Repositories
{
    public class MarshalsRepository(AutoTestContext autoTestContext) : IMarshalsRepository
    {
        IQueryable<Marshal> IMarshalsRepository.GetByEmail(string emailAddress)
        {
            return autoTestContext.Marshals!.Where(a => a.Email == emailAddress);
        }

        IQueryable<Marshal> IMarshalsRepository.GetByEventId(ulong eventId)
        {
            return autoTestContext.Marshals!.Where(a => a.EventId == eventId);
        }

        Task<ulong> IMarshalsRepository.GetMarshalIdByEmail(ulong eventId, string emailAddress, CancellationToken cancellationToken)
        {
            return autoTestContext.Marshals!.Where(a => a.EventId == eventId && a.Email == emailAddress).Select(a => a.MarshalId).SingleAsync(cancellationToken);
        }

        Task<Marshal?> IMarshalsRepository.GetById(ulong eventId, ulong marshalId, CancellationToken cancellationToken)
        {
            return autoTestContext.Marshals!.SingleOrDefaultAsync(a => a.EventId == eventId && a.MarshalId == marshalId, cancellationToken);
        }

        async Task IMarshalsRepository.Upsert(Marshal marshal, CancellationToken cancellationToken)
        {
            await autoTestContext.Marshals!.Upsert(marshal, a => a.MarshalId == marshal.MarshalId, cancellationToken);
            await autoTestContext.SaveChangesAsync(cancellationToken);
        }

        Task IMarshalsRepository.Remove(Marshal marshal, CancellationToken cancellationToken)
        {
            autoTestContext.Marshals!.Remove(marshal);
            return autoTestContext.SaveChangesAsync(cancellationToken);
        }
    }
}
