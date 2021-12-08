using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Enums;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetProfileHandler : IRequestHandler<GetProfile, Profile>
    {
        private readonly AutoTestContext autoTestContext;

        public GetProfileHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<Profile> IRequestHandler<GetProfile, Profile>.Handle(GetProfile request, CancellationToken cancellationToken)
        {
            var found = await this.autoTestContext.Users!.Where(a => a.EmailAddress == request.EmailAddress).SingleOrDefaultAsync(cancellationToken);
            return found == null ? new Profile(request.EmailAddress, "", "", Age.Senior) : found;
        }
    }
}
