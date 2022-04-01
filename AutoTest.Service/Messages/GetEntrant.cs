// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class GetEntrant : IRequest<Entrant?>
    {
        public GetEntrant(ulong eventId, ulong entrantId)
        {
            EventId = eventId;
            EntrantId = entrantId;
        }

        public ulong EventId { get; }
        public ulong EntrantId { get; }
    }
}
