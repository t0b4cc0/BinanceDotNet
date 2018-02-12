//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace BinanceExchange.Console
//{
//    class ExampleWebsocket
//    {
//        /// <summary>
//        /// Build local Depth cache from WebSocket and API Call example.
//        /// </summary>
//        /// <param name="client"></param>
//        /// <returns></returns>
//        private static async Task<Dictionary<string, DepthCacheObject>> BuildLocalDepthCache(IBinanceClient client)
//        {
//            // Code example of building out a Dictionary local cache for a symbol using deltas from the WebSocket
//            var localDepthCache = new Dictionary<string, DepthCacheObject> {{ "BNBBTC", new DepthCacheObject()
//            {
//                Asks = new Dictionary<decimal, decimal>(),
//                Bids = new Dictionary<decimal, decimal>(),
//            }}};
//            var bnbBtcDepthCache = localDepthCache["BNBBTC"];

//            // Get Order Book, and use Cache
//            var depthResults = await client.GetOrderBook("BNBBTC", true, 100);
//            //Populate our depth cache
//            depthResults.Asks.ForEach(a =>
//            {
//                if (a.Quantity != 0.00000000M)
//                {
//                    bnbBtcDepthCache.Asks.Add(a.Price, a.Quantity);
//                }
//            });
//            depthResults.Bids.ForEach(a =>
//            {
//                if (a.Quantity != 0.00000000M)
//                {
//                    bnbBtcDepthCache.Bids.Add(a.Price, a.Quantity);
//                }
//            });



//            // Store the last update from our result set;
//            long lastUpdateId = depthResults.LastUpdateId;
//            using (var binanceWebSocketClient = new DisposableBinanceWebSocketClient(client))
//            {
//                binanceWebSocketClient.ConnectToDepthWebSocket("BNBBTC", data =>
//                {
//                    if (lastUpdateId < data.UpdateId)
//                    {
//                        data.BidDepthDeltas.ForEach((bd) =>
//                        {
//                            CorrectlyUpdateDepthCache(bd, bnbBtcDepthCache.Bids);
//                        });
//                        data.AskDepthDeltas.ForEach((ad) =>
//                        {
//                            CorrectlyUpdateDepthCache(ad, bnbBtcDepthCache.Asks);
//                        });
//                    }
//                    lastUpdateId = data.UpdateId;
//                    System.Console.Clear();
//                    System.Console.WriteLine($"{JsonConvert.SerializeObject(bnbBtcDepthCache, Formatting.Indented)}");
//                    System.Console.SetWindowPosition(0, 0);
//                });

//                Thread.Sleep(8000);
//            }
//            return localDepthCache;
//        }

//        //// Manual WebSocket usage
//        //var manualBinanceWebSocket = new InstanceBinanceWebSocketClient(client);
//        //var socketId = manualBinanceWebSocket.ConnectToDepthWebSocket("ETHBTC", b =>
//        //{
//        //    System.Console.Clear();
//        //    System.Console.WriteLine($"{JsonConvert.SerializeObject(b.BidDepthDeltas, Formatting.Indented)}");
//        //    System.Console.SetWindowPosition(0, 0);
//        //});

//        /// <summary>
//        /// Advanced approach to building local Kline Cache from WebSocket and API Call example (refactored)
//        /// </summary>
//        /// <param name="binanceClient">The BinanceClient instance</param>
//        /// <param name="symbol">The Symbol to request</param>
//        /// <param name="interval">The interval for Klines</param>
//        /// <param name="klinesCandlesticksRequest">The initial request for Klines</param>
//        /// <param name="webSocketConnectionFunc">The function to determine exiting the websocket (can be timeout or Func based on external params)</param>
//        /// <param name="cacheObject">The cache object. Must always be provided, and can exist with data.</param>
//        /// <returns></returns>
//        public static async Task BuildAndUpdateLocalKlineCache(IBinanceClient binanceClient,
//            string symbol,
//            KlineInterval interval,
//            GetKlinesCandlesticksRequest klinesCandlesticksRequest,
//            WebSocketConnectionFunc webSocketConnectionFunc,
//            Dictionary<string, KlineCacheObject> cacheObject)
//        {
//            Guard.AgainstNullOrEmpty(symbol);
//            Guard.AgainstNull(webSocketConnectionFunc);
//            Guard.AgainstNull(klinesCandlesticksRequest);
//            Guard.AgainstNull(cacheObject);

//            long epochTicks = new DateTime(1970, 1, 1).Ticks;

//            if (cacheObject.ContainsKey(symbol))
//            {
//                if (cacheObject[symbol].KlineInterDictionary.ContainsKey(interval))
//                {
//                    throw new Exception(
//                        "Symbol and Interval pairing already provided, please use a different interval/symbol or pair.");
//                }
//                cacheObject[symbol].KlineInterDictionary.Add(interval, new KlineIntervalCacheObject());
//            }
//            else
//            {
//                var klineCacheObject = new KlineCacheObject
//                {
//                    KlineInterDictionary = new Dictionary<KlineInterval, KlineIntervalCacheObject>()
//                };
//                cacheObject.Add(symbol, klineCacheObject);
//                cacheObject[symbol].KlineInterDictionary.Add(interval, new KlineIntervalCacheObject());
//            }

//            // Get Kline Results, and use Cache
//            var startTimeKeyTime = (klinesCandlesticksRequest.StartTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond;
//            var klineResults = await binanceClient.GetKlinesCandlesticks(klinesCandlesticksRequest);

//            var oneMinKlineCache = cacheObject[symbol].KlineInterDictionary[interval];
//            oneMinKlineCache.TimeKlineDictionary = new Dictionary<long, KlineCandleStick>();
//            var instanceKlineCache = oneMinKlineCache.TimeKlineDictionary;
//            //Populate our kline cache with initial results
//            klineResults.ForEach(k =>
//            {
//                instanceKlineCache.Add(((k.OpenTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond), new KlineCandleStick()
//                {
//                    Close = k.Close,
//                    High = k.High,
//                    Low = k.Low,
//                    Open = k.Open,
//                    Volume = k.Volume,
//                });
//            });

//            // Store the last update from our result set;
//            using (var binanceWebSocketClient = new DisposableBinanceWebSocketClient(binanceClient))
//            {
//                binanceWebSocketClient.ConnectToKlineWebSocket(symbol, interval, data =>
//                {
//                    var keyTime = (data.Kline.StartTime.Ticks - epochTicks) / TimeSpan.TicksPerSecond;
//                    var klineObj = new KlineCandleStick()
//                    {
//                        Close = data.Kline.Close,
//                        High = data.Kline.High,
//                        Low = data.Kline.Low,
//                        Open = data.Kline.Open,
//                        Volume = data.Kline.Volume,
//                    };
//                    if (!data.Kline.IsBarFinal)
//                    {
//                        if (keyTime < startTimeKeyTime)
//                        {
//                            return;
//                        }

//                        TryAddUpdateKlineCache(instanceKlineCache, keyTime, klineObj);
//                    }
//                    else
//                    {
//                        TryAddUpdateKlineCache(instanceKlineCache, keyTime, klineObj);
//                    }
//                    System.Console.Clear();
//                    System.Console.WriteLine($"{JsonConvert.SerializeObject(instanceKlineCache, Formatting.Indented)}");
//                    System.Console.SetWindowPosition(0, 0);
//                });
//                if (webSocketConnectionFunc.IsTimout)
//                {
//                    Thread.Sleep(webSocketConnectionFunc.Timeout);
//                }
//                else
//                {
//                    while (true)
//                    {
//                        if (!webSocketConnectionFunc.ExitFunction())
//                        {
//                            // Throttle Application
//                            Thread.Sleep(100);
//                        }
//                        else
//                        {
//                            break;
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
