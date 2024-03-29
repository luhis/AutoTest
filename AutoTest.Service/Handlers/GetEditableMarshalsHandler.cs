﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class GetEditableMarshalsHandler(IMarshalsRepository marshalsRepository) : IRequestHandler<GetEditableMarshals, IEnumerable<ulong>>
    {
        Task<IEnumerable<ulong>> IRequestHandler<GetEditableMarshals, IEnumerable<ulong>>.Handle(GetEditableMarshals request, CancellationToken cancellationToken)
        {
            return marshalsRepository.GetByEmail(request.EmailAddress).Select(a => a.MarshalId).ToEnumerableAsync(cancellationToken);
        }
    }
}
