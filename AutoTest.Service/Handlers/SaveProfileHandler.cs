using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveProfileHandler : IRequestHandler<SaveProfile, string>
    {
        private readonly IProfileRepository autoTestContext;

        public SaveProfileHandler(IProfileRepository autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<string> IRequestHandler<SaveProfile, string>.Handle(SaveProfile request, CancellationToken cancellationToken)
        {
            await autoTestContext.Upsert(request.Profile, cancellationToken);
            return request.Profile.EmailAddress;
        }
    }
}
