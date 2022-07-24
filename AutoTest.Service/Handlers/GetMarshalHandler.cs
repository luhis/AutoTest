// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AutoTest.Service.Handlers
{
    public class GetMarshalHandler : IRequestHandler<GetMarshal, Marshal>
    {
        private readonly AutoTestContext autoTestContext;

        public GetMarshalHandler(AutoTestContext autoTestContext)
        {
            this.autoTestContext = autoTestContext;
        }

        Task<Marshal> IRequestHandler<GetMarshal, Marshal>.Handle(GetMarshal request, CancellationToken cancellationToken)
        {
            return this.autoTestContext.Marshals!.Where(a => a.EventId == request.EventId && a.MarshalId == request.MarshalId).OrderByDescending(a => a.FamilyName).SingleAsync(cancellationToken);
        }
    }
}
