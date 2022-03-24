
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class AddNotificationShould
    {
        private readonly IRequestHandler<AddNotification, MediatR.Unit> sut;
        private readonly MockRepository mr;
        private readonly Mock<ISignalRNotifier> notifier;
        private readonly AutoTestContext context;

        public AddNotificationShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            context = new AutoTestContext(new DbContextOptionsBuilder<AutoTestContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options);
            notifier = mr.Create<ISignalRNotifier>();
            sut = new AddNotificationHandler(context, notifier.Object);
        }

        [Fact]
        public async Task ShouldNotifyOnNewNotification()
        {
            context.Notifications!.Should().BeEmpty();
            var notification = new Notification(1, 2, "message", new System.DateTime(2000, 1, 1), "test user");
            notifier.Setup(a => a.NewNotification(notification, CancellationToken.None)).Returns(Task.CompletedTask);
            var res = await sut.Handle(new(notification), CancellationToken.None);

            mr.VerifyAll();
            context.Notifications!.Should().NotBeEmpty();
        }
    }
}
