using System.Linq;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Persistence.Repositories;
using AutoTest.Unit.Test.Fixtures;
using AwesomeAssertions;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class NotificationRepositoryShould
{
    [Fact]
    public async Task GetNotificationsReturnsNotificationsForEvent()
    {
        using var db = InMemDbFixture.GetDbContext();
        INotificationsRepository sut = new NotificationRepository(db);

        db.Notifications.AddRange(
            new Notification(1, 10, "Message 1", new System.DateTime(2024, 1, 1), "admin@test.com"),
            new Notification(2, 10, "Message 2", new System.DateTime(2024, 1, 2), "admin@test.com"),
            new Notification(3, 20, "Other Event", new System.DateTime(2024, 1, 3), "admin@test.com"));
        await db.SaveChangesAsync(TestContext.Current.CancellationToken);

        var result = (await sut.GetNotifications(10, TestContext.Current.CancellationToken)).ToArray();

        result.Should().HaveCount(2);
        result[0].Message.Should().Be("Message 2");
        result[1].Message.Should().Be("Message 1");
    }

    [Fact]
    public async Task GetNotificationsReturnsEmptyWhenNoneExist()
    {
        using var db = InMemDbFixture.GetDbContext();
        INotificationsRepository sut = new NotificationRepository(db);

        var result = (await sut.GetNotifications(999, TestContext.Current.CancellationToken)).ToArray();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task AddNotificationAddsToDatabase()
    {
        using var db = InMemDbFixture.GetDbContext();
        INotificationsRepository sut = new NotificationRepository(db);

        var notification = new Notification(1, 10, "New message", new System.DateTime(2024, 6, 1), "admin@test.com");
        await sut.AddNotification(notification, TestContext.Current.CancellationToken);

        var result = await db.Notifications.FindAsync(new object[] { 1UL }, TestContext.Current.CancellationToken);
        result.Should().NotBeNull();
        result!.Message.Should().Be("New message");
    }
}
