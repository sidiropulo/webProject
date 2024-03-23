// Controllers/CryptoController.cs
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CryptoInfo.Models;
using CryptoInfo.Services;
using Bybit.Net.Clients;
using Bybit.Net;

namespace CryptoInfo.Controllers
{
    public class CryptoController : Controller
    {
        public async Task SubscribeToTickerUpdatesAsync()
        {
            var client = new BybitSocketClient();

            // Подписываемся на обновления котировок для выбранной пары
            var subscribeResult = await client.V5SpotApi.SubscribeToTickerUpdatesAsync("ETHUSDT", update =>
            {
                if (update.Data != null)
                {
                    // Получаем последнюю цену и объем торгов
                    decimal lastPrice = update.Data.LastPrice;
                    decimal lowPrice = update.Data.LowPrice24h;
                    decimal highPrice = update.Data.HighPrice24h;

                    // Далее вы можете обработать полученные данные, например, сохранить их в базу данных
                    // или передать на frontend через SignalR или AJAX
                    Console.WriteLine(lastPrice + "\t" + lowPrice + "\t" + highPrice);

                    // Пример вызова метода для сохранения данных в модели или другую логику
                    SaveDataToDatabase(lastPrice, lowPrice, highPrice);
                }
            });

        }

        private void SaveDataToDatabase(decimal lastPrice, decimal lowPrice, decimal highPrice)
        {
            // Здесь добавьте логику сохранения данных в базу данных или другие действия с данными
            // Пример: использование Entity Framework для сохранения данных в базу данных
            // dbContext.Add(new PriceData { LastPrice = lastPrice, LowPrice24h = lowPrice, HighPrice24h = highPrice });
            // dbContext.SaveChanges();
        }
    }
}
