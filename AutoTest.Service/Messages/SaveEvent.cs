using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SaveEvent : IRequest<ulong>
    {
        public SaveEvent(Event @event)
        {
            this.Event = @event;
        }

        public Event Event { get; }
    }
}
