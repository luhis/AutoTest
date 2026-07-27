using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using AutoTest.Domain.Repositories;

namespace AutoTest.Persistence.Repositories;

public class FileRepository(BlobContainerClient containerClient) : IFileRepository
{
    public async Task<string> GetMaps(ulong eventId, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"maps/{eventId}");
        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return string.Empty;
        }
        var response = await blobClient.DownloadContentAsync(cancellationToken);
        return response.Value.Content.ToString();
    }

    public async Task<string> GetRegs(ulong eventId, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"regs/{eventId}");
        if (!await blobClient.ExistsAsync(cancellationToken))
        {
            return string.Empty;
        }
        var response = await blobClient.DownloadContentAsync(cancellationToken);
        return response.Value.Content.ToString();
    }

    public async Task<string> SaveMaps(ulong eventId, string data, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"maps/{eventId}");
        await blobClient.UploadAsync(new BinaryData(Encoding.UTF8.GetBytes(data)), overwrite: true, cancellationToken);
        return string.Empty;
    }

    public async Task<string> SaveRegs(ulong eventId, string data, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"regs/{eventId}");
        await blobClient.UploadAsync(new BinaryData(Encoding.UTF8.GetBytes(data)), overwrite: true, cancellationToken);
        return string.Empty;
    }

    public async Task<bool> HasRegs(ulong eventId, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"regs/{eventId}");
        return await blobClient.ExistsAsync(cancellationToken);
    }

    public async Task<bool> HasMaps(ulong eventId, CancellationToken cancellationToken)
    {
        var blobClient = containerClient.GetBlobClient($"maps/{eventId}");
        return await blobClient.ExistsAsync(cancellationToken);
    }
}
