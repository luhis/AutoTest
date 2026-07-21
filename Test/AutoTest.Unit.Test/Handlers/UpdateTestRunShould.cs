using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AwesomeAssertions.ArgumentMatchers.Moq;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class UpdateTestRunShould
{
    private readonly IRequestHandler<UpdateTestRun> _sut;
    private readonly MockRepository _mr;
    private readonly Mock<IEventNotifier> _notifier;
    private readonly Mock<ITestRunsRepository> _testRuns;

    public UpdateTestRunShould()
    {
        _mr = new MockRepository(MockBehavior.Strict);
        _notifier = _mr.Create<IEventNotifier>();
        _testRuns = _mr.Create<ITestRunsRepository>();
        _sut = new UpdateTestRunHandler(_testRuns.Object, _notifier.Object);
    }

    [Fact]
    public async Task ShouldNotifyOnUpdatedTestRun()
    {
        var entrantId = 5ul;
        var marshalId = 6ul;
        var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
        var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
        tr.SetPenalties(penalties);
        _notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _testRuns.Setup(a => a.UpdateTestRun(Its.EquivalentTo(tr), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId, penalties), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }

    [Fact(Skip = "Todo not implemented")]
    public async Task NotChangeCreatedDate()
    {
        var entrantId = 5ul;
        var marshalId = 6ul;
        var penalties = new[] { new Penalty(Domain.Enums.PenaltyEnum.Late, 1) };
        var tr = new TestRun(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 1), marshalId);
        tr.SetPenalties(penalties);
        _notifier.Setup(a => a.NewTestRun(Its.EquivalentTo(tr), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);
        _testRuns.Setup(a => a.UpdateTestRun(Its.EquivalentTo(tr), TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(1, 2, 3, 4, entrantId, new System.DateTime(2000, 1, 2), marshalId, penalties), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
    }
}
