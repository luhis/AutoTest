using System.Threading;
using System.Threading.Tasks;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveProfileHandler : IRequestHandler<SaveProfile, string>
    {
        private readonly AutoTestContext autoTestContext;

        public SaveProfileHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<string> IRequestHandler<SaveProfile, string>.Handle(SaveProfile request, CancellationToken cancellationToken)
        {
            await autoTestContext.Users!.Upsert(request.Profile, a => a.EmailAddress == request.EmailAddress, cancellationToken);
            await this.autoTestContext.SaveChangesAsync(cancellationToken);
            return request.Profile.EmailAddress;
        }
    }
}
