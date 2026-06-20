using System.Collections.Generic;
using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class GetClubs : IRequest<IEnumerable<Club>>
{
}
