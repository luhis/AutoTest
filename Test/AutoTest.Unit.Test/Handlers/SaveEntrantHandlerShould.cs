﻿
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveEntrantHandlerShould
    {
        private readonly IRequestHandler<SaveEntrant, Entrant> sut;
        private readonly MockRepository mr;
        private readonly Mock<IEntrantsRepository> entrantsRepository;

        public SaveEntrantHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            entrantsRepository = mr.Create<IEntrantsRepository>();
            sut = new SaveEntrantHandler(entrantsRepository.Object);
        }

        [Fact]
        public async Task ShouldNotOverwritePaymentMethodWhenNone()
        {
            var entrantId = 1ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", 2, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrant.SetPayment(new Payment());

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", 2, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrantsRepository.Setup(a => a.GetById(entrantId, CancellationToken.None)).Returns(Task.FromResult<Entrant?>(entrantFromDb));
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotOverwritePaymentMethodWhenSome()
        {
            var entrantId = 1ul;
            var entrant = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", 2, "BRMC", 123456, Domain.Enums.Age.Senior);

            var entrantFromDb = new Entrant(entrantId, 123, "name", "familyName", "a@a.com", "A", 2, "BRMC", 123456, Domain.Enums.Age.Senior);
            entrantFromDb.SetPayment(new Payment());
            entrantsRepository.Setup(a => a.GetById(entrantId, CancellationToken.None)).Returns(Task.FromResult<Entrant?>(entrantFromDb));
            entrantsRepository.Setup(a => a.Upsert(entrant, CancellationToken.None)).Returns(Task.CompletedTask);
            var se = new SaveEntrant(entrant);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
            res.Payment.Should().NotBeNull();
        }
    }
}
