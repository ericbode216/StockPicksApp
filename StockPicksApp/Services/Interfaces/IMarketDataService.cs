public interface IMarketDataService
{
    public Task<decimal> GetHistoricalPrice(
        string stockTicker,
        DateTime stockBuyDate
    );
    public Task<decimal> GetLatestPrice(string stockTicker);
}