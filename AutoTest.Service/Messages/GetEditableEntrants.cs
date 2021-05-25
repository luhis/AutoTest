// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MediatR;

namespace AutoTest.Service.Messages
{
    public record GetEditableEntrants : IRequest<IEnumerable<ulong>>
    {
        public string EmailAddress { get; }

        public GetEditableEntrants(string emailAddress)
        {
            EmailAddress = emailAddress;
        }
    }
}
