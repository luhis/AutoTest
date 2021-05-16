using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveMarshalHandler : IRequestHandler<SaveMarshal, Marshal>
    {
        private readonly AutoTestContext autoTestContext;

        public SaveMarshalHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<Marshal> IRequestHandler<SaveMarshal, Marshal>.Handle(SaveMarshal request, CancellationToken cancellationToken)
        {
            await autoTestContext.Marshals!.Upsert(request.Marshal, a => a.MarshalId == request.Marshal.MarshalId, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Marshal;
        }
    }
}
