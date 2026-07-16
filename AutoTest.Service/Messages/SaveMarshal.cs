using AutoTest.Domain.StorageModels;
using Mediator;

namespace AutoTest.Service.Messages;

public class SaveMarshal : IRequest<Marshal>
{
    public SaveMarshal(Marshal marshal)
    {
        Marshal = marshal;
    }

    public Marshal Marshal { get; }
}
