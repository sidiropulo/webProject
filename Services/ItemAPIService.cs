using CryptoInfo.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace CryptoInfo.Services
{
    public class ItemAPIService
    {
        private readonly HttpClient _httpClient;

        public ItemAPIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Item> GetItemInfo(string apiUrl)
        {
            var response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Item>(content);
            }

            return null;
        }
    }
}

