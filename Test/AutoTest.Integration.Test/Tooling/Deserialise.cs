using System.Net.Http;
using System.Text.Json;

namespace AutoTest.Integration.Test.Tooling
{
    internal static class Deserialise
    {
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        public static async System.Threading.Tasks.Task<T> DeserialiseAsync<T>(this HttpResponseMessage message)
        {

            var content = await message.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, options)!;
        }
    }
}
