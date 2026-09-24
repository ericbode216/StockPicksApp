

public class PickReasonsService : IPickReasonsService
{
    private readonly IPickReasonsRepository repository;

    public PickReasonsService(IPickReasonsRepository repository)
    {
        this.repository = repository;
    }

    public Task<List<PickReasonEntity>> GetAll()
    {
        return repository.GetAll();
    }

    public Task<List<PickReasonEntity>> GetByStockId(int stockPickId)
    {
        return repository.GetByStockId(stockPickId);
    }

    public async Task<PickReasonEntity> Add(PickReasonDto pickReasonDto)
    {
        var pickReason = new PickReasonEntity();
        pickReason.StockId = pickReasonDto.StockId;
        pickReason.Reason = pickReasonDto.Reason;
        pickReason.Date = DateTime.Parse(pickReasonDto.Date);

        return await repository.Add(pickReason);
    }
    public Task<PickReasonEntity> Update(PickReasonDto pickReasonDto)
    {
        throw new NotImplementedException();
    }

    public Task<PickReasonEntity> Delete(int id)
    {
        throw new NotImplementedException();
    }


    
}