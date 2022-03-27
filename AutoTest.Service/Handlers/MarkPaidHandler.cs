using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class MarkPaidHandler : IRequestHandler<MarkPaid>
    {
        private readonly IEntrantsRepository _entrantsRepository;

        public MarkPaidHandler(IEntrantsRepository entrantsRepository)
        {
            _entrantsRepository = entrantsRepository;
        }


        async Task<Unit> IRequestHandler<MarkPaid, Unit>.Handle(MarkPaid request, CancellationToken cancellationToken)
        {
            var found = (await _entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken))!;// await this._autoTestContext.Entrants!.SingleAsync(a => a.EventId == request.EventId && a.EntrantId == request.EntrantId, cancellationToken);
            found.SetPayment(request.Payment);
            await _entrantsRepository.Update(found, cancellationToken);
            return Unit.Value;
        }
    }
}
