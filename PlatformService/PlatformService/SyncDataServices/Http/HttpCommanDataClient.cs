using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommanDataClient : ICommanDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommanDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
           _config = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                 JsonSerializer.Serialize(platform),
                 Encoding.UTF8,
                 "application/json"
                );

            var response = await _httpClient.PostAsync($"{_config["CommandService"]}{_config["CommandPlatformUrl"]}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("---> Sync POST to CommandService was Ok!");

            } else
            {
                Console.WriteLine("---> Sync POST to CommandService was not Ok!");
            }
        }
    }
}
