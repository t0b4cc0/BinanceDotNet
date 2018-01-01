using BinanceExchange.API.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceExchange.API.Utility
{
    public class KlinesIntervalToTimeSpan
    {
        public static TimeSpan Help(KlineInterval interval)
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
                    return TimeSpan.FromDays((365.25)/12);
                default:
                    return TimeSpan.MinValue;
            }
        }
    }
}
