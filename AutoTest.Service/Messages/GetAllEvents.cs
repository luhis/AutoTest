namespace AutoTest.Service.Messages
{
    using System.Collections.Generic;
    using AutoTest.Domain.StorageModels;
    using MediatR;

    public class GetAllEvents : IRequest<IEnumerable<Event>>
    {

    }
}
