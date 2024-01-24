using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Interfaces;
using AutoTest.Service.Messages;
using AutoTest.Web.Hubs;
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
        private readonly Mock<IAuthorisationNotifier> authorisationNotifier;
        private readonly Mock<IMarshalsRepository> marshalRepository;

        public SaveMarshalHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            authorisationNotifier = mr.Create<IAuthorisationNotifier>();
            marshalRepository = mr.Create<IMarshalsRepository>();
            sut = new SaveMarshalHandler(marshalRepository.Object, authorisationNotifier.Object);
        }

        [Fact]
        public async Task SaveMarshalExisting()
        {
            var marshalId = 1ul;
            var eventId = 2ul;
            var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");

            marshalRepository.Setup(a => a.GetById(eventId, marshalId, CancellationToken.None)).ReturnsAsync(marshal);
            marshalRepository.Setup(a => a.Upsert(marshal, CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveMarshal(marshal);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }

        [Fact]
        public async Task SaveMarshalExistingUpdateEmail()
        {
            var marshalId = 1ul;
            var eventId = 2ul;
            var marshal = new Marshal(marshalId, "name", "familyName", "a@a.com", eventId, 123456, "");
            var marshal2 = new Marshal(marshalId, "name", "familyName", "b@a.com", eventId, 123456, "");

            marshalRepository.Setup(a => a.GetById(eventId, marshalId, CancellationToken.None)).ReturnsAsync(marshal);
            marshalRepository.Setup(a => a.Upsert(marshal2, CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "b@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.RemoveEventMarshal(eventId, Its.EquivalentTo(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.NewEventMarshal(eventId, Its.EquivalentTo(new[] { "b@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveMarshal(marshal2);
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
            authorisationNotifier.Setup(a => a.NewEventMarshal(eventId, Its.EquivalentTo<IEnumerable<string>>(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);
            authorisationNotifier.Setup(a => a.AddEditableMarshal(marshalId, Its.EquivalentTo(new[] { "a@a.com" }), CancellationToken.None)).Returns(Task.CompletedTask);

            var se = new SaveMarshal(marshal);
            var res = await sut.Handle(se, CancellationToken.None);

            mr.VerifyAll();
        }
    }
}
