﻿using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddNotificationShould
    {
        private readonly IRequestHandler<AddNotification> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEventNotifier> notifier;
        private readonly Mock<INotificationsRepository> notificationRepository;

        public AddNotificationShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            notifier = mr.Create<IEventNotifier>();
            notificationRepository = mr.Create<INotificationsRepository>();
            sut = new AddNotificationHandler(notificationRepository.Object, notifier.Object);
        }

        [Fact]
        public async Task NotifyOnNewNotification()
        {
            var notification = new Notification(1, 2, "message", new System.DateTime(2000, 1, 1), "test user");
            notifier.Setup(a => a.NewNotification(notification, CancellationToken.None)).Returns(Task.CompletedTask);
            notificationRepository.Setup(a => a.AddNotification(notification, CancellationToken.None)).Returns(Task.CompletedTask);

            await sut.Handle(new(notification), CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
