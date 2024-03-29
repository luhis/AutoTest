﻿// Licensed to the .NET Foundation under one or more agreements.
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
    public class GetClubHandler(IClubsRepository clubRepository) : IRequestHandler<GetClub, Club?>
    {
        Task<Club?> IRequestHandler<GetClub, Club?>.Handle(GetClub request, CancellationToken cancellationToken)
        {
            return clubRepository.GetById(request.ClubId, cancellationToken);
        }
    }
}
