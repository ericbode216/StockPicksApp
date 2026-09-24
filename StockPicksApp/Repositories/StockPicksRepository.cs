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
    Task<StockPickEntity> Delete(StockPickEntity stockPick);
}

public class StockPicksRepository : IStockPicksRepository
{



    private readonly StockPicksDbContext context;

    public StockPicksRepository(StockPicksDbContext context, IConfiguration configuration)
    {
        this.context = context;
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
        context.StockPicks.Update(stockPick);
        await context.SaveChangesAsync();
        return stockPick;
    }

    public async Task<StockPickEntity> Delete(StockPickEntity stockPick)
    {
        Console.WriteLine("inside repository delete");
        context.StockPicks.Remove(stockPick);
        await context.SaveChangesAsync();
        return stockPick;
    }
}