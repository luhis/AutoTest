// using Azure.Storage.Blobs;
// using System;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Azure.Identity;

// namespace AutoTest.Web.Extensions
// {
//     public static class IServiceCollectionExtensions
//     {
//         public static IServiceCollection AddBlobStorage(this IServiceCollection services, IConfiguration configuration)
//         {
//             ArgumentNullException.ThrowIfNull(services);

//             // Endpoint should look like "https://{account_name}.blob.core.windows.net" except for emulator
//             var endpoint = configuration.GetValue<string>("Endpoint") ?? "UseDevelopmentStorage=true;";
//             var containerName = configuration.GetValue<string>("ContainerName");

//             services.AddSingleton<BlobServiceClient>(provider =>
//             {
//                 // Create the credential, you can use connection string instead
//                 var credential = new DefaultAzureCredential();
//                 /*
//                  * TokenCredential cannot be used with non https scheme.
//                  * So we have to check if we are local and use a connection string instead.
//                  * https://github.com/Azure/azure-sdk/issues/2195
//                  */
//                 var isLocal = endpoint.StartsWith("http://127.0.0.1", StringComparison.OrdinalIgnoreCase) || endpoint.Contains("UseDevelopmentStorage");
//                 return isLocal
//                         ? new BlobServiceClient(connectionString: "UseDevelopmentStorage=true;")
//                         : new BlobServiceClient(serviceUri: new Uri(endpoint), credential: credential);
//             });

//             services.AddScoped<BlobContainerClient>(provider =>
//             {
//                 var serviceClient = provider.GetRequiredService<BlobServiceClient>();
//                 return serviceClient.GetBlobContainerClient(containerName);
//             });

//             //services.AddSingleton<IActionResultExecutor<BlobStorageResult>, BlobStorageResultExecutor>();

//             return services;
//         }
//     }
// }
