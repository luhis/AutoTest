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
        private const string ContainerName = "autotest";
        private readonly BlobContainerClient _client = service.GetBlobContainerClient(ContainerName);

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

        private async Task CreateContainerIfNotExists(CancellationToken cancellationToken)
        {
            if (!await _client.ExistsAsync(cancellationToken))
            {
                await service.CreateBlobContainerAsync(ContainerName, cancellationToken: cancellationToken);
            }
        }

        async Task IFileRepository.SaveMaps(ulong eventId, string data, CancellationToken cancellationToken)
        {
            await CreateContainerIfNotExists(cancellationToken);
            var reference = _client.GetBlobClient(GetMapFileName(eventId));
            await reference.UploadAsync(BinaryData.FromString(data), true, cancellationToken);
        }

        async Task IFileRepository.SaveRegs(ulong eventId, string data, CancellationToken cancellationToken)
        {
            await CreateContainerIfNotExists(cancellationToken);
            var reference = _client.GetBlobClient(GetRegsFileName(eventId));
            await reference.UploadAsync(BinaryData.FromString(data), true, cancellationToken);
        }

        async Task IFileRepository.DeleteRegs(ulong eventId, CancellationToken cancellationToken)
        {
            await CreateContainerIfNotExists(cancellationToken);
            var reference = _client.GetBlobClient(GetRegsFileName(eventId));
            await reference.DeleteAsync(cancellationToken: cancellationToken);
        }

        async Task IFileRepository.DeleteMaps(ulong eventId, CancellationToken cancellationToken)
        {
            await CreateContainerIfNotExists(cancellationToken);
            var reference = _client.GetBlobClient(GetMapFileName(eventId));
            await reference.DeleteAsync(cancellationToken: cancellationToken);
        }
    }
}
