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
    public class GetMarshalHandler : IRequestHandler<GetMarshal, Marshal>
    {
        private readonly IMarshalsRepository marshalsRepository;

        public GetMarshalHandler(IMarshalsRepository marshalsRepository)
        {
            this.marshalsRepository = marshalsRepository;
        }

        Task<Marshal> IRequestHandler<GetMarshal, Marshal>.Handle(GetMarshal request, CancellationToken cancellationToken)
        {
            return this.marshalsRepository.GetById(request.EventId, request.MarshalId, cancellationToken);
        }
    }
}
