using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveMarshalHandler : IRequestHandler<SaveMarshal, Marshal>
    {
        private readonly AutoTestContext autoTestContext;
        private readonly ISignalRNotifier signalRNotifier;

        public SaveMarshalHandler(AutoTestContext autoTestContext, ISignalRNotifier signalRNotifier)
        {
            this.autoTestContext = autoTestContext;
            this.signalRNotifier = signalRNotifier;
        }

        async Task<Marshal> IRequestHandler<SaveMarshal, Marshal>.Handle(SaveMarshal request, CancellationToken cancellationToken)
        {
            var existing = autoTestContext.Marshals!.SingleOrDefault(a => a.MarshalId == request.Marshal.MarshalId);
            await autoTestContext.Marshals!.Upsert(request.Marshal, a => a.MarshalId == request.Marshal.MarshalId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            if (existing == null || !existing.Email.Equals(request.Marshal.Email, System.StringComparison.InvariantCultureIgnoreCase))
            {
                await signalRNotifier.NewEventMarshal(request.Marshal.EventId, new[] { request.Marshal.Email });
                if (existing != null)
                {
                    await signalRNotifier.RemoveEventMarshal(request.Marshal.EventId, new[] { existing.Email });
                }
            }
            return request.Marshal;
        }
    }
}
