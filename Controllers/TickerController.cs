using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bybit.Net.Clients;
using CryptoInfo.Models; // Подключите необходимые пространства имён
using System;

namespace CryptoInfo.Controllers
{
    public class TickerController : Controller
    {
        private readonly BybitSocketClient _client;

        public TickerController(BybitSocketClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickerData()
        {

            // Вызываем метод для получения последних данных
            var tickerData = await GetLatestTickerData();
            return Ok(tickerData);
        }

        private async Task<TickerData> GetLatestTickerData()
        {
            try
            {
                TickerData tickerData = new TickerData();
                Random random = new Random();
                tickerData.LastPrice = Convert.ToDecimal(random.NextDouble() * 1000);  
                tickerData.LowPrice24h = Convert.ToDecimal(random.NextDouble() * 500); 
                tickerData.HighPrice24h = Convert.ToDecimal(random.NextDouble() * 1500);
                var update = await _client.V5SpotApi.SubscribeToTickerUpdatesAsync("ETHUSDT", update =>
                {
                    if (update.Data != null)
                    {
                        tickerData.LastPrice = update.Data.Volume24h;
                        tickerData.LowPrice24h = update.Data.LowPrice24h;
                        tickerData.HighPrice24h = update.Data.HighPrice24h;

                        // Добавим вывод для проверки обновления значений
                    }
                });

                return tickerData;
            }

            catch (OperationCanceledException ex)
            {
                // Обработка отмены операции, например, логирование ошибки
                Console.WriteLine($"Operation canceled: {ex.Message}");
                return null; // Возвращаем null или другое значение по умолчанию
            }
            catch (Exception ex)
            {
                // Обработка других ошибок, например, логирование ошибки
                Console.WriteLine($"Error getting ticker data: {ex.Message}");
                return null; // Возвращаем null или другое значение по умолчанию
            }
        }
    }
}
