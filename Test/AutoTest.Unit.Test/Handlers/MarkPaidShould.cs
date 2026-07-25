using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using AutoTest.Unit.Test.MockData;
using Mediator;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers;

public class MarkPaidShould
{
    private readonly MockRepository _mr = new(MockBehavior.Strict);
    private readonly Mock<IEntrantsRepository> _entrantsRepository;
    private readonly IRequestHandler<MarkPaid> _sut;

    private readonly Payment _testPayment = new(new System.DateTime(2000, 1, 1), Domain.Enums.PaymentMethod.Paypal, new System.DateTime(2000, 2, 2), "test@test.com");

    public MarkPaidShould()
    {
        _entrantsRepository = _mr.Create<IEntrantsRepository>();
        _sut = new MarkPaidHandler(_entrantsRepository.Object);
    }

    [Fact]
    public async Task MarkNotPaid()
    {
        var entrantId = 1ul;
        var eventId = 22ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        entrant.SetPayment(_testPayment);
        _entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrant);
        _entrantsRepository.Setup(a => a.Update(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(eventId, entrantId, null), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
        _entrantsRepository.Verify(a => a.Update(It.Is<Entrant>(a => a.Payment == null), TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task MarkPaid()
    {
        var entrantId = 1ul;
        var eventId = 22ul;
        var entrant = Models.GetEntrant(entrantId, eventId);
        _entrantsRepository.Setup(a => a.GetById(eventId, entrantId, TestContext.Current.CancellationToken)).ReturnsAsync(entrant);
        _entrantsRepository.Setup(a => a.Update(entrant, TestContext.Current.CancellationToken)).Returns(Task.CompletedTask);

        await _sut.Handle(new(eventId, entrantId, _testPayment), TestContext.Current.CancellationToken);

        _mr.VerifyAll();
        _entrantsRepository.Verify(a => a.Update(It.Is<Entrant>(a => a.Payment != null), TestContext.Current.CancellationToken));
    }
}
