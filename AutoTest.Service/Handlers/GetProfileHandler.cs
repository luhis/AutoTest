using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetProfileHandler : IRequestHandler<GetProfile, User>
    {
        private readonly AutoTestContext autoTestContext;

        public GetProfileHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        async Task<User> IRequestHandler<GetProfile, User>.Handle(GetProfile request, CancellationToken cancellationToken)
        {
            return await this.autoTestContext.Users.FirstAsync(cancellationToken);
        }
    }
}
