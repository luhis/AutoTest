using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public record MarkPaid(ulong EventId, ulong EntrantId, Payment? Payment) : IRequest;
