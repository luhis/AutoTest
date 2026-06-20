// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using Mediator;

namespace AutoTest.Service.Handlers;

public sealed class GetEventHandler(IEventsRepository entrantsRepository) : IRequestHandler<GetEvent, Event?>
{
    public async ValueTask<Event?> Handle(GetEvent request, CancellationToken cancellationToken)
    {
        return await entrantsRepository.GetById(request.EventId, cancellationToken);
    }
}
