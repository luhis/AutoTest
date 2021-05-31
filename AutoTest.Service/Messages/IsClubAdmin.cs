﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using MediatR;

namespace AutoTest.Service.Messages
{
    public class IsClubAdmin : IRequest<bool>
    {
        public IsClubAdmin(ulong eventId, string emailAddress)
        {
            EventId = eventId;
            EmailAddress = emailAddress;
        }

        public ulong EventId { get; }
        public string EmailAddress { get; }
    }
}
