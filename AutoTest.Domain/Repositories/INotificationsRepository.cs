﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.StorageModels;

namespace AutoTest.Domain.Repositories
{
    public interface INotificationsRepository
    {
        Task AddNotificaiton(Notification notification, CancellationToken cancellationToken);
        Task<IEnumerable<Notification>> GetNotificaitons(ulong eventId, CancellationToken cancellationToken);
    }
}
