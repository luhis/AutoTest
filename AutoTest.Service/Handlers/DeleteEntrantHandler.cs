using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class DeleteEntrantHandler : IRequestHandler<DeleteEntrant>
    {
        private readonly IEntrantsRepository _autoTestContext;

        public DeleteEntrantHandler(IEntrantsRepository entrantsRepository)
        {
            _autoTestContext = entrantsRepository;
        }

        async Task IRequestHandler<DeleteEntrant>.Handle(DeleteEntrant request, CancellationToken cancellationToken)
        {
            var found = await this._autoTestContext.GetById(request.EventId, request.EntrantId, cancellationToken);
            if (found == null)
            {
                throw new NullReferenceException();
            }
            await this._autoTestContext.Delete(found, cancellationToken);
        }
    }
}
