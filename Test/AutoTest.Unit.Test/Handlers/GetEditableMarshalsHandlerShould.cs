﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using AutoTest.Domain.StorageModels;
using AutoTest.Service.Handlers;
using AutoTest.Service.Messages;
using FluentAssertions;
using MediatR;
using MockQueryable;
using Moq;
using Xunit;

namespace AutoTest.Unit.Test.Handlers
{
    public class GetEditableMarshalsHandlerShould
    {
        private readonly MockRepository mr;
        private readonly IRequestHandler<GetEditableMarshals, IEnumerable<ulong>> sut;
        private readonly Mock<IMarshalsRepository> profileRepository;

        public GetEditableMarshalsHandlerShould()
        {
            mr = new MockRepository(MockBehavior.Strict);
            profileRepository = mr.Create<IMarshalsRepository>();
            sut = new GetEditableMarshalsHandler(profileRepository.Object);
        }

        [Fact]
        public async Task GetMarshals()
        {
            var eventId = 1ul;
            var marshals = new[] {
                new Marshal(1, "b", "b", "a@a.com", eventId, 212312, ""),
                new Marshal(2, "a", "a", "a@a.com", eventId, 212312, "")
            };
            var mock = marshals.BuildMock();
            profileRepository.Setup(a => a.GetByEmail("test@test.com")).Returns(mock);

            var res = await sut.Handle(new("test@test.com"), CancellationToken.None);

            res.Should().BeEquivalentTo(marshals.Select(a => a.MarshalId));
            mr.VerifyAll();
        }
    }
}
