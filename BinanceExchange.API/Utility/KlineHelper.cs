using BinanceExchange.API.Enums;
using BinanceExchange.API.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceExchange.API.Utility
{
    public class KlineHelper
    {
        public static KlineCandleStickResponse MakeDummyKline()
        {
            return new KlineCandleStickResponse()
            {
                Close = 666,
                Open = 666,
                High = 666,
                Low = 666,
                Volume = 666,
                QuoteAssetVolume = 666,
                NumberOfTrades = 666,
                TakerBuyBaseAssetVolume = 666,
                TakerBuyQuoteAssetVolume = 666,
            };
        }

        public static bool IsDummyKline(KlineCandleStickResponse candle)
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + candle.Close.GetHashCode();
                hash = hash * 31 + candle.Open.GetHashCode();
                hash = hash * 31 + candle.High.GetHashCode();
                hash = hash * 31 + candle.Low.GetHashCode();
                hash = hash * 31 + candle.Volume.GetHashCode();
                hash = hash * 31 + candle.QuoteAssetVolume.GetHashCode();
                hash = hash * 31 + candle.NumberOfTrades.GetHashCode();
                hash = hash * 31 + candle.TakerBuyBaseAssetVolume.GetHashCode();
                hash = hash * 31 + candle.TakerBuyQuoteAssetVolume.GetHashCode();

                int hash2 = 17;
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();
                hash2 = hash2 * 31 + 666m.GetHashCode();

                return hash == hash2;
            }
        }

        public static bool ContainsDummyKline(IEnumerable<KlineCandleStickResponse> candles)
        {
            foreach (var candle in candles)
                if (IsDummyKline(candle))
                    return true;

            return false;
        }

        public static TimeSpan KlineIntervalToTimespan(KlineInterval interval)
        {
            switch (interval)
            {
                case KlineInterval.OneMinute:
                    return TimeSpan.FromMinutes(1);
                case KlineInterval.ThreeMinutes:
                    return TimeSpan.FromMinutes(3);
                case KlineInterval.FiveMinutes:
                    return TimeSpan.FromMinutes(5);
                case KlineInterval.FifteenMinutes:
                    return TimeSpan.FromMinutes(15);
                case KlineInterval.ThirtyMinutes:
                    return TimeSpan.FromMinutes(30);
                case KlineInterval.OneHour:
                    return TimeSpan.FromHours(1);
                case KlineInterval.TwoHours:
                    return TimeSpan.FromHours(2);
                case KlineInterval.FourHours:
                    return TimeSpan.FromHours(4);
                case KlineInterval.SixHours:
                    return TimeSpan.FromHours(6);
                case KlineInterval.EightHours:
                    return TimeSpan.FromHours(8);
                case KlineInterval.TwelveHours:
                    return TimeSpan.FromHours(12);
                case KlineInterval.OneDay:
                    return TimeSpan.FromDays(1);
                case KlineInterval.ThreeDays:
                    return TimeSpan.FromDays(3);
                case KlineInterval.OneWeek:
                    return TimeSpan.FromDays(7);
                case KlineInterval.OneMonth:
                    return TimeSpan.FromDays((365.25) / 12);
                default:
                    return TimeSpan.MinValue;
            }
        }
    }
}
