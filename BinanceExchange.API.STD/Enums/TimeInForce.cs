namespace BinanceExchange.API.Enums
{
    public enum TimeInForce
    {
        /// <summary>
        /// GTC (Good ‘Til Canceled) – ensures that your entire order is executed
        /// GTC orders may be broken up into partial orders if sufficient liquidity isn’t available to fill the entire order at the best available price.
        /// </summary>
        GTC,
        /// <summary>
        /// IOC (Immediate or Cancel) – fills as much of your order as possible at the best available price
        /// If the entire order cannot be filled at the best available price, the remainder cancels.
        /// </summary>
        IOC,
    }
}