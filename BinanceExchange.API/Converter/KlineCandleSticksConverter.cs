using System;
using System.Linq;
using BinanceExchange.API.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinanceExchange.API.Converter
{
    public class KlineCandleSticksConverter : JsonConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var candle = value as KlineCandleStickResponse;
            //var jarr = new JArray() {
            writer.WriteStartArray();

            writer.WriteValue((long)candle.OpenTime.Subtract(Epoch).TotalMilliseconds);
            writer.WriteValue(candle.Open);
            writer.WriteValue(candle.High);
            writer.WriteValue(candle.Low);
            writer.WriteValue(candle.Close);
            writer.WriteValue(candle.Volume);
            writer.WriteValue((long)candle.CloseTime.Subtract(Epoch).TotalMilliseconds);
            writer.WriteValue(candle.QuoteAssetVolume);
            writer.WriteValue(candle.NumberOfTrades);
            writer.WriteValue(candle.TakerBuyBaseAssetVolume);
            writer.WriteValue(candle.TakerBuyQuoteAssetVolume);
            //};
            

            //foreach (var val in jarr)
            //val

            writer.WriteEndArray();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var klineCandlesticks = JArray.Load(reader);

            return new KlineCandleStickResponse
            {
                OpenTime = Epoch.AddMilliseconds((long)klineCandlesticks.ElementAt(0)),
                Open = (decimal)klineCandlesticks.ElementAt(1),
                High = (decimal)klineCandlesticks.ElementAt(2),
                Low = (decimal)klineCandlesticks.ElementAt(3),
                Close = (decimal)klineCandlesticks.ElementAt(4),
                Volume = (decimal)klineCandlesticks.ElementAt(5),
                CloseTime = Epoch.AddMilliseconds((long)klineCandlesticks.ElementAt(6)),
                QuoteAssetVolume = (decimal)klineCandlesticks.ElementAt(7),
                NumberOfTrades = (int)klineCandlesticks.ElementAt(8),
                TakerBuyBaseAssetVolume = (decimal)klineCandlesticks.ElementAt(9),
                TakerBuyQuoteAssetVolume = (decimal)klineCandlesticks.ElementAt(10),
            };
        }

        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }
    }
}