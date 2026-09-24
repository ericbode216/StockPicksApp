//external API calls to TIINGO or other places

using System.Text.Json;

public class MarketDataService : IMarketDataService
{
    private static string _token = string.Empty;

    public MarketDataService(IConfiguration configuration)
    {
        _token= configuration["TiingoToken"];
        
    }
    public async Task<decimal> GetHistoricalPrice(string stockTicker, DateTime stockBuyDate)
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.tiingo.com");

        client.DefaultRequestHeaders.UserAgent.ParseAdd("Csharp-Sample-App");

        try
        {
            //stock exchange closed some days. 7 days should ensure the stock exchange was open at least one day
            DateTime buyDatePlus7 = stockBuyDate.AddDays(7);

            // Send the GET request to a specific endpoint (e.g., .NET Foundation repositories)
            HttpResponseMessage response = await client.GetAsync(
                $"/tiingo/daily/{stockTicker}/prices?&startDate={stockBuyDate.ToString("yyyy-MM-dd")}&endDate={buyDatePlus7.ToString("yyyy-MM-dd")}&token={_token}"
            );
            // Check if the request was successful
            response.EnsureSuccessStatusCode();
            

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            //deserializes array of json objects
            var list = JsonSerializer.Deserialize<List<TiingoHistoricalPrice>>(responseBody);
            if (list.Count == 0)
            {
                //no price data from tiingo
                return -1;
            }

            //used to get first object
            TiingoHistoricalPrice tiingoHistoricalPrice = list.Find(x => x.close != null);

            return Convert.ToDecimal(tiingoHistoricalPrice.close);
        }
        catch (HttpRequestException e)
        {
            return -1;
        }
    }

    public async Task<decimal> GetLatestPrice(string stockTicker)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri("https://api.tiingo.com");

        client.DefaultRequestHeaders.UserAgent.ParseAdd("Csharp-Sample-App");

        try
        {
            // Send the GET request to a specific endpoint (e.g., .NET Foundation repositories)
            HttpResponseMessage response = await client.GetAsync(
                $"/tiingo/daily/{stockTicker}/prices?token={_token}"
            );
            // Check if the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            //deserializes array of json objects
            var list = JsonSerializer.Deserialize<List<TiingoLatestPrice>>(responseBody);

            //used to get first object
            TiingoLatestPrice tiingoLatestPrice = list.Find(x => x.adjClose != null);

            return Convert.ToDecimal(tiingoLatestPrice.adjClose);
        }
        catch
        {
            return -1;
        }
    }
}