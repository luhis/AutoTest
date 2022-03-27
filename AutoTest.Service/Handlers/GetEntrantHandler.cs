// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEntrantHandler : IRequestHandler<GetEntrant, Entrant>
    {
        private readonly IEntrantsRepository entrantsRepository;

        public GetEntrantHandler(IEntrantsRepository entrantsRepository)
        {
            this.entrantsRepository = entrantsRepository;
        }

        Task<Entrant> IRequestHandler<GetEntrant, Entrant>.Handle(GetEntrant request, CancellationToken cancellationToken)
        {
            return this.entrantsRepository.GetById(request.EventId, request.EntrantId, cancellationToken)!;
        }
    }
}
