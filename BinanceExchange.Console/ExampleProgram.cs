using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BinanceExchange.API;
using BinanceExchange.API.Caching;
using BinanceExchange.API.Client;
using BinanceExchange.API.Enums;
using BinanceExchange.API.Market;
using BinanceExchange.API.Models.Request;
using BinanceExchange.API.Models.Response;
using BinanceExchange.API.Models.Response.Error;
using BinanceExchange.API.Models.Websocket;
using BinanceExchange.API.Utility;
//using BinanceExchange.API.Websockets;
//using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using NLog;
//using NLog.Extensions.Logging;
//using LogLevel = NLog.LogLevel;

namespace BinanceExchange.Console
{
    /// <summary>
    /// This Console app provides a number of examples of utilising the BinanceDotNet library
    /// </summary>
    public class ExampleProgram
    {
        public static async Task Main(string[] args)
        {
            ////Logging Configuration. 
            ////Ensure that `nlog.config` is configured as you want, and is copied to output directory.
            //var loggerFactory = new LoggerFactory();
            //loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            ////This utilises the nlog.config from the build directory
            //loggerFactory.ConfigureNLog("nlog.config");
            ////For the sakes of this example we are outputting only fatal logs, debug being the lowest.
            //LogManager.GlobalThreshold = LogLevel.Fatal;
            //var logger = LogManager.GetLogger("*");

            //Provide your configuration and keys here, this allows the client to function as expected.
            string apiKey = "YOUR_API_KEY";
            string secretKey = "YOUR_SECRET_KEY";

            System.Console.WriteLine("--------------------------");
            System.Console.WriteLine("BinanceExchange API - Tester");
            System.Console.WriteLine("--------------------------");

            //Initialise the general client client with config
            var client = new BinanceClient(new ClientConfiguration()
            {
                ApiKey = apiKey,
                SecretKey = secretKey,
                //Logger = logger,
            });

            System.Console.WriteLine("Interacting with Binance...");

            bool DEBUG_ALL = false;

            /*
             *  Code Examples - Make sure you adjust value of DEBUG_ALL
             */
            if (DEBUG_ALL)
            {
                // Test the Client
                await client.TestConnectivity();

                // Get All Orders
                var allOrdersRequest = new AllOrdersRequest()
                {
                    Symbol = "ETHBTC",
                    Limit = 5,
                };
                allOrdersRequest = new AllOrdersRequest()
                {
                    Symbol = TradingPairSymbols.BTCPairs.ETH_BTC,
                    Limit = 5,
                };
                var allOrders = await client.GetAllOrders(allOrdersRequest);                // Get All Orders

                // Get the order book, and use the cache
                var orderBook = await client.GetOrderBook("ETHBTC", true);

                //// Cancel an order
                //var cancelOrder = await client.CancelOrder(new CancelOrderRequest()
                //{
                //    NewClientOrderId = 123456,
                //    OrderId = 523531,
                //    OriginalClientOrderId = 23525,
                //    Symbol = "ETHBTC",
                //});

                //// Create an order with varying options
                //var createOrder = await client.CreateOrder(new CreateOrderRequest()
                //{
                //    IcebergQuantity = 100,
                //    Price = 230,
                //    Quantity = 0.6m,
                //    Side = OrderSide.Buy,
                //    Symbol = "ETHBTC",
                //    Type = OrderType.Market,
                //});

                // Get account information
                var accountInformation = await client.GetAccountInformation(3500);

                // Get account trades
                var accountTrades = await client.GetAccountTrades(new AllTradesRequest()
                {
                    FromId = 352262,
                    Symbol = "ETHBTC",
                });

                // Get a list of Compressed aggregate trades with varying options
                var aggTrades = await client.GetCompressedAggregateTrades(new GetCompressedAggregateTradesRequest()
                {
                    StartTime = DateTime.UtcNow.AddDays(-1),
                    Symbol = "ETHBTC",
                });

                // Get current open orders for the specified symbol
                var currentOpenOrders = await client.GetCurrentOpenOrders(new CurrentOpenOrdersRequest()
                {
                    Symbol = "ETHBTC",
                });

                // Get daily ticker
                var dailyTicker = await client.GetDailyTicker("ETHBTC");

                // Get Symbol Order Book Ticket
                var symbolOrderBookTicker = await client.GetSymbolOrderBookTicker();

                // Get Symbol Order Price Ticker
                var symbolOrderPriceTicker = await client.GetSymbolsPriceTicker();

                // Query a specific order on Binance
                var orderQuery = await client.QueryOrder(new QueryOrderRequest()
                {
                    OrderId = 5425425,
                    Symbol = "ETHBTC",
                });

                // Firing off a request and catching all the different exception types.
                try
                {
                    accountTrades = await client.GetAccountTrades(new AllTradesRequest()
                    {
                        FromId = 352262,
                        Symbol = "ETHBTC",
                    });
                }
                catch (BinanceBadRequestException badRequestException)
                {

                }
                catch (BinanceServerException serverException)
                {

                }
                catch (BinanceTimeoutException timeoutException)
                {

                }
                catch (BinanceException unknownException)
                {
                    
                }
            }

            // Start User Data Stream, ping and close
            var userData = await client.StartUserDataStream();
            await client.KeepAliveUserDataStream(userData.ListenKey);
            await client.CloseUserDataStream(userData.ListenKey);




            #region Advanced Examples           
            // This builds a local Kline cache, with an initial call to the API and then continues to fill
            // the cache with data from the WebSocket connection. It is quite an advanced example as it provides 
            // additional options such as an Exit Func<T> or timeout, and checks in place for cache instances. 
            // You could provide additional logic here such as populating a database, ping off more messages, or simply
            // timing out a fill for the cache.
            var dict = new Dictionary<string, KlineCacheObject>();
            //await BuildAndUpdateLocalKlineCache(client, "BNBBTC", KlineInterval.OneMinute,
            //    new GetKlinesCandlesticksRequest()
            //    {
            //        StartTime = DateTime.UtcNow.AddHours(-1),
            //        EndTime = DateTime.UtcNow,
            //        Interval = KlineInterval.OneMinute,
            //        Symbol = "BNBBTC"
            //    }, new WebSocketConnectionFunc(15000), dict);

            // This builds a local depth cache from an initial call to the API and then continues to fill 
            // the cache with data from the WebSocket
            //var localDepthCache = await BuildLocalDepthCache(client);
            // Build the Buy Sell volume from the results
            //var volume = ResultTransformations.CalculateTradeVolumeFromDepth("BNBBTC", localDepthCache);

            #endregion
            System.Console.WriteLine("Complete.");
            Thread.Sleep(6000);
            //manualBinanceWebSocket.CloseWebSocketInstance(socketId);
            System.Console.ReadLine();
        }

        private static void TryAddUpdateKlineCache(Dictionary<long, KlineCandleStick> primary, long keyTime, KlineCandleStick klineObj)
        {
            if (primary.ContainsKey(keyTime))
            {
                primary[keyTime] = klineObj;
            }
            else
            {
                primary.Add(keyTime, klineObj);
            }
        }

        private static void CorrectlyUpdateDepthCache(TradeResponse bd,  Dictionary<decimal, decimal> depthCache)
        {
            const decimal defaultIgnoreValue = 0.00000000M;

            if (depthCache.ContainsKey(bd.Price))
            {
                if (bd.Quantity == defaultIgnoreValue)
                {
                    depthCache.Remove(bd.Price);
                }
                else
                {
                    depthCache[bd.Price] = bd.Quantity;
                }
            }
            else
            {
                if (bd.Quantity != defaultIgnoreValue)
                {
                    depthCache[bd.Price] = bd.Quantity;
                }
            }
        }
    }
}
