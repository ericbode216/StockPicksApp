using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IStockPicksRepositiory
{
    Task<List<StockPickEntity>> GetAll();
    Task<StockPickEntity> Get(int id);
    Task<StockPickEntity> Add(StockPickAddDto stockPickDto);
    Task<StockPickEntity> Update(StockPickUpdateDto stockPick);
    Task<StockPickEntity> Delete(int id);
}

public class StockPicksRepository : IStockPicksRepositiory
{
    private readonly StockPicksDbContext context;

    public StockPicksRepository(StockPicksDbContext context)
    {
        this.context = context;
    }

    public async Task<List<StockPickEntity>> GetAll()
    {
        return await context.StockPicks.ToListAsync();
    }

    public async Task<StockPickEntity> Get(int id)
    {
        return await context.StockPicks.SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<StockPickEntity> Add(StockPickAddDto stockPickDto)
    {
        var entity = new StockPickEntity();
        entity.StockTicker = stockPickDto.StockTicker;
        entity.StockBuyDate = DateTime.Parse(stockPickDto.StockBuyDate);

        ////Call 3rd Paty API
        entity.StockBuyPrice = await GetHistoricalPriceTiingo(
            entity.StockTicker,
            entity.StockBuyDate
        );
        entity.IndexTicker = stockPickDto.IndexTicker;
        entity.IndexBuyPrice = await GetHistoricalPriceTiingo(
            entity.IndexTicker,
            entity.StockBuyDate
        );

        ////Call 3rd Paty API
        entity.StockCurrentPrice = await GetLatestPriceTiingo(entity.StockTicker);
        entity.IndexCurrentPrice = await GetLatestPriceTiingo(entity.IndexTicker);

        entity.StockCurrentDate = DateTime.Now;
        DoPercentCalculations(entity);

        context.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<StockPickEntity> Update(StockPickUpdateDto stockPickDto)
    {
        var foundStockPick = await context.StockPicks.FindAsync(stockPickDto.Id);
        if (foundStockPick == null)
        {
            throw new ArgumentException($"Error updating stockpick {stockPickDto.Id}");
        }
        foundStockPick.StockTicker = stockPickDto.StockTicker;
        foundStockPick.StockBuyDate = DateTime.Parse(stockPickDto.StockBuyDate);
        foundStockPick.IndexTicker = stockPickDto.IndexTicker;

        ////Call 3rd Paty API
        foundStockPick.StockBuyPrice = await GetHistoricalPriceTiingo(
            foundStockPick.StockTicker,
            foundStockPick.StockBuyDate
        );
        foundStockPick.IndexBuyPrice = await GetHistoricalPriceTiingo(
            foundStockPick.IndexTicker,
            foundStockPick.StockBuyDate
        );

        //Call 3rd Paty API
        foundStockPick.StockCurrentPrice = await GetLatestPriceTiingo(foundStockPick.StockTicker);
        foundStockPick.IndexCurrentPrice = await GetLatestPriceTiingo(foundStockPick.IndexTicker);
        foundStockPick.StockCurrentDate = DateTime.Now;
        DoPercentCalculations(foundStockPick);

        context.Entry(foundStockPick).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return foundStockPick;
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
                $"/tiingo/daily/{stockTicker}/prices?token=619f1b00679d6398aba5fe0125f7a298ce8bd7cc"
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
                $"/tiingo/daily/{stockTicker}/prices?&startDate={stockBuyDate.ToString("yyyy-MM-dd")}&endDate={buyDatePlus7.ToString("yyyy-MM-dd")}&token=619f1b00679d6398aba5fe0125f7a298ce8bd7cc"
            );
            // Check if the request was successful
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            //deserializes array of json objects
            var list = JsonSerializer.Deserialize<List<TiingoHistoricalPrice>>(responseBody);

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
