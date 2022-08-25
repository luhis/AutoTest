using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using FluentAssertions.ArgumentMatchers.Moq;
using MediatR;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class SaveMarshalHandlerShould
    {
        private readonly IRequestHandler<SaveMarshal, Marshal> sut;
        private readonly MockRepository mr;
        private readonly Mock<ISignalRNotifier> signalRNotifier;
        private readonly Mock<IMarshalsRepository> marshalRepository;

        public SaveMarshalHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            signalRNotifier = mr.Create<ISignalRNotifier>();
            marshalRepository = mr.Create<IMarshalsRepository>();
            sut = new SaveMarshalHandler(marshalRepository.Object, signalRNotifier.Object);
        }

        [Fact]
        public async Task SaveMarshalExisting()
        {
            var marshalId = 1ul;
            var eventId = 2ul;
            var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

            marshalRepository.Setup(a => a.GetById(eventId, marshalId, CancellationToken.None)).ReturnsAsync(marshal);
            marshalRepository.Setup(a => a.Upsert(marshal, CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveMarshal(marshal);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task SaveMarshalNew()
        {
            var marshalId = 1ul;
            var eventId = 2ul;
            var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

            marshalRepository.Setup(a => a.GetById(eventId, marshalId, CancellationToken.None)).ReturnsAsync((Marshal?)null);
            marshalRepository.Setup(a => a.Upsert(marshal, CancellationToken.None)).Returns(Task.CompletedTask);
            signalRNotifier.Setup(a => a.NewEventMarshal(eventId, Its.EquivalentTo<IEnumerable<string>>(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveMarshal(marshal);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
