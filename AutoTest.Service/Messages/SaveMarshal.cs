using AutoTest.Domain.StorageModels;
using MediatR;

namespace AutoTest.Service.Messages
{
    public class SaveMarshal : IRequest<Marshal>
    {
        public SaveMarshal(Marshal marshal)
        {
            this.Marshal = marshal;
        }

        public Marshal Marshal { get; }
    }
}
