using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace AutoTest.Service.Handlers;

public sealed class GetAccessHandler(RootAdminEmails rootAdminEmails, IServiceScopeFactory scopeFactory) : IRequestHandler<GetAccess, AccessModel>
{
    public async ValueTask<AccessModel> Handle(GetAccess request, CancellationToken cancellationToken)
    {
        var adminClubsTask = SendInScope(new GetAdminClubs(request.EmailAddress), cancellationToken);
        var marshalEventsTask = SendInScope(new GetMarshalEvents(request.EmailAddress), cancellationToken);
        var editableEntrantsTask = SendInScope(new GetEditableEntrants(request.EmailAddress), cancellationToken);
        var editableMarshalsTask = SendInScope(new GetEditableMarshals(request.EmailAddress), cancellationToken);

        await Task.WhenAll(adminClubsTask, marshalEventsTask, editableEntrantsTask, editableMarshalsTask);

        return new AccessModel(
            rootAdminEmails.Contains(request.EmailAddress),
            request.IsAuthenticated,
            await adminClubsTask,
            await marshalEventsTask,
            await editableEntrantsTask,
            await editableMarshalsTask);
    }

    private async Task<T> SendInScope<T>(IRequest<T> request, CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(request, cancellationToken);
    }
}
