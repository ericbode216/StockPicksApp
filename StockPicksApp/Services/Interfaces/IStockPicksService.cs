public interface IStockPicksService
{
    Task<List<StockPickEntity>> GetAll();
    Task<StockPickEntity> GetById(int id);
    Task<StockPickEntity> Add(StockPickAddDto stockPickDto);
    Task<StockPickEntity> Update(StockPickUpdateDto stockPick);
    Task<StockPickEntity> Delete(int id);
}