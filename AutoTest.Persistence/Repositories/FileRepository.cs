using System;
using System.Threading;
using System.Threading.Tasks;
using AutoTest.Domain.Repositories;

namespace AutoTest.Persistence.Repositories
{
    public class FileRepository : IFileRepository, IDisposable
    {
        //private DocumentClient _client;
        //private string _databaseId = "AutoTestDB";
        //private string _collectionId = "AutoTestContext";

        //public FileRepository(IConfiguration configuration)
        //{
        //    var config = configuration.GetSection("Cosmos");
        //    var endpoint = config.GetValue<string>("Endpoint") ?? "";
        //    var key = config.GetValue<string>("Key") ?? "";

        //    _client = new DocumentClient(new Uri(endpoint), key);
        //}

        private static string GetMapFileName(ulong eventId) => $"Maps/{eventId}";
        private static string GetRegsFileName(ulong eventId) => $"Regs/{eventId}";

        Task<string> IFileRepository.GetMaps(ulong eventId, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
            //var results = new List<string>();
            //try
            //{
            //    var query = _client
            //        .CreateDocumentQuery<string>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
            //            new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            //    while (query.HasMoreResults) results.AddRange(await query.ExecuteNextAsync<string>(cancellationToken));
            //}
            //catch (DocumentQueryException ex)
            //{
            //    Console.WriteLine(ex.StatusCode);
            //}

            //return results.First();
        }

        Task<string> IFileRepository.GetRegs(ulong eventId, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
            //var results = new List<string>();
            //try
            //{
            //    var query = _client
            //        .CreateDocumentQuery<string>(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
            //            new FeedOptions { MaxItemCount = -1 }).AsDocumentQuery();
            //    while (query.HasMoreResults) results.AddRange(await query.ExecuteNextAsync<string>(cancellationToken));
            //}
            //catch (DocumentQueryException ex)
            //{
            //    Console.WriteLine(ex.StatusCode);
            //}

            //return results.First();
        }

        Task<string> IFileRepository.SaveMaps(ulong eventId, string data, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
            //var id = GetMapFileName(eventId);
            //try
            //{
            //    var updateResponse =
            //        await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), data, cancellationToken: cancellationToken);
            //    if (updateResponse.StatusCode != HttpStatusCode.OK) return "Not OK";

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return "";
        }

        Task<string> IFileRepository.SaveRegs(ulong eventId, string data, CancellationToken cancellationToken)
        {
            return Task.FromResult("");
            //var id = GetRegsFileName(eventId);
            //try
            //{
            //    var updateResponse =
            //        await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), data, cancellationToken: cancellationToken);
            //    if (updateResponse.StatusCode != HttpStatusCode.OK) return "Not OK";

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return "";
        }

        public void Dispose()
        {
            //_client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
