using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class AddNotification : IRequest
    {
        public AddNotification(Notification notification)
        {
            Notification = notification;
        }

        public Notification Notification { get; }
    }
}
