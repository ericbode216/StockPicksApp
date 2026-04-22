using Microsoft.EntityFrameworkCore;

public interface IPickReasonsRepository
{
    Task<List<PickReasonEntity>> GetAll();
    Task<List<PickReasonEntity>> Get(int stockPickId);
    Task<PickReasonEntity> Add(PickReasonEntity pickReason);
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

    public async Task<List<PickReasonEntity>> Get(int stockPickId)
    {
        return await context.PickReasons.Where(p => p.StockId == stockPickId).ToListAsync();
    }

    public async Task<PickReasonEntity> Add(PickReasonEntity pickReason)
    {
        var entity = new PickReasonEntity();
        entity.StockId = pickReason.StockId;
        entity.Reason = pickReason.Reason;

        context.Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public Task<PickReasonEntity> Delete(int id)
    {
        throw new NotImplementedException();
    }

    

    

    public Task<PickReasonEntity> Update(PickReasonEntity pickReason)
    {
        throw new NotImplementedException();
    }
}