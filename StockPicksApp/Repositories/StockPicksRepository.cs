using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IStockPicksRepository
{
    Task<List<StockPickEntity>> GetAll();
    Task<StockPickEntity> GetById(int id);
    Task<StockPickEntity> Add(StockPickEntity stockPick);
    Task<StockPickEntity> Update(StockPickEntity stockPick);
    Task<StockPickEntity> Delete(int id);
}

public class StockPicksRepository : IStockPicksRepository
{

    private static string _token = string.Empty;


    private readonly StockPicksDbContext context;

    public StockPicksRepository(StockPicksDbContext context, IConfiguration configuration)
    {
        this.context = context;
        _token= configuration["TiingoToken"];
    }

    public async Task<List<StockPickEntity>> GetAll()
    {
        return await context.StockPicks.ToListAsync();
    }

    public async Task<StockPickEntity> GetById(int id)
    {
        return await context.StockPicks.SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<StockPickEntity> Add(StockPickEntity stockPick)
    {
        context.Add(stockPick);
        await context.SaveChangesAsync();
        return stockPick;
    }

    public async Task<StockPickEntity> Update(StockPickEntity stockPick)
    {
        context.Entry(stockPick).State = EntityState.Modified;
        context.Update(stockPick);
        //await context.SaveChangesAsync();
        return stockPick;
    }

    public async Task<StockPickEntity> Delete(int stockId)
    {
        var entity = await context.StockPicks.FindAsync(stockId);
        context.Remove(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public static async Task<decimal> GetLatestPriceTiingo(string stockTicker)
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
        catch (HttpRequestException e)
        {
            return -1;
        }
    }

    public static async Task<decimal> GetHistoricalPriceTiingo(
        string stockTicker,
        DateTime stockBuyDate
    )
    {
        using var client = new HttpClient();
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

    public void DoPercentCalculations(StockPickEntity entity)
    {
        entity.StockTotalPercentGain = (entity.StockCurrentPrice / entity.StockBuyPrice - 1) * 100;
        entity.IndexTotalPercentGain = (entity.IndexCurrentPrice / entity.IndexBuyPrice - 1) * 100;

        TimeSpan ts = entity.StockCurrentDate.Subtract(entity.StockBuyDate);

        decimal years = ts.Days / 365M;
        Console.WriteLine("years: " + years);

        entity.StockAnnualPercentGain =
            (decimal)(
                Math.Pow(
                    (double)(entity.StockCurrentPrice / entity.StockBuyPrice),
                    (double)(1 / years)
                ) - 1
            ) * 100;

        entity.IndexAnnualPercentGain =
            (decimal)(
                Math.Pow(
                    (double)(entity.IndexCurrentPrice / entity.IndexBuyPrice),
                    (double)(1 / years)
                ) - 1
            ) * 100;
    }
}
