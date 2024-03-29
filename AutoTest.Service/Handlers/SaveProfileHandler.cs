﻿using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Service.Messages;
using MediatR;

namespace AutoTest.Service.Handlers
{
    public class SaveProfileHandler(IProfileRepository profileRepository) : IRequestHandler<SaveProfile, string>
    {
        async Task<string> IRequestHandler<SaveProfile, string>.Handle(SaveProfile request, CancellationToken cancellationToken)
        {
            await profileRepository.Upsert(request.Profile, cancellationToken);
            return request.Profile.EmailAddress;
        }
    }
}
