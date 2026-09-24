using Microsoft.EntityFrameworkCore;

public interface IPickReasonsRepository
{
    Task<List<PickReasonEntity>> GetAll();
    Task<List<PickReasonEntity>> GetByStockId(int stockPickId);
    Task<PickReasonEntity> Add(PickReasonEntity pickReasonEntity);
    Task<PickReasonEntity> Update(PickReasonEntity pickReason);
    Task<PickReasonEntity> Delete(int id);
}

public class PickReasonsRepository : IPickReasonsRepository
{
    private readonly StockPicksDbContext context;
    public PickReasonsRepository(StockPicksDbContext context)
    {
        this.context = context;
    }

    public async Task<List<PickReasonEntity>> GetAll()
    {
        return await context.PickReasons.ToListAsync();
    }

    public async Task<List<PickReasonEntity>> GetByStockId(int stockPickId)
    {
        return await context.PickReasons.Where(p => p.StockId == stockPickId).ToListAsync();
    }

    public async Task<PickReasonEntity> Add(PickReasonEntity pickReason)
    {
        context.Add(pickReason);
        await context.SaveChangesAsync();
        return pickReason;
    }

    public Task<PickReasonEntity> Update(PickReasonEntity pickReason)
    {
        throw new NotImplementedException();
    }

    public Task<PickReasonEntity> Delete(int id)
    {
        throw new NotImplementedException();
    }
}