using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AutoTest.Persistence.Repositories
{
    public class BlobFileRepository(BlobServiceClient service) : IFileRepository
    {
        private readonly BlobContainerClient _client = service.GetBlobContainerClient("AutoTest");

        private static string GetMapFileName(ulong eventId) => $"Maps/{eventId}";
        private static string GetRegsFileName(ulong eventId) => $"Regs/{eventId}";

        async Task<string> IFileRepository.GetMaps(ulong eventId, CancellationToken cancellationToken)
        {
            var res = await Download(GetMapFileName(eventId), cancellationToken);
            return res.Value.Content.ToString();
        }

        async Task<string> IFileRepository.GetRegs(ulong eventId, CancellationToken cancellationToken)
        {
            var res = await Download(GetRegsFileName(eventId), cancellationToken);
            return res.Value.Content.ToString();
        }

        private Task<Response<BlobDownloadResult>> Download(string name, CancellationToken cancellationToken)
        {
            var reference = _client.GetBlobClient(name);
            return reference.DownloadContentAsync(cancellationToken);
        }

        Task IFileRepository.SaveMaps(ulong eventId, string data, CancellationToken cancellationToken)
        {
            var reference = _client.GetBlobClient(GetMapFileName(eventId));
            return reference.UploadAsync(BinaryData.FromString(data), true, cancellationToken);
        }

        Task IFileRepository.SaveRegs(ulong eventId, string data, CancellationToken cancellationToken)
        {
            var reference = _client.GetBlobClient(GetRegsFileName(eventId));
            return reference.UploadAsync(BinaryData.FromString(data), true, cancellationToken);
        }
    }
}
