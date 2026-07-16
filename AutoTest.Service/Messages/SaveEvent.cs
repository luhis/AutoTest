using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class SaveEvent : IRequest<ulong>
{
    public SaveEvent(Event @event)
    {
        Event = @event;
    }

    public Event Event { get; }
}
