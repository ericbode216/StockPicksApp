public interface IPickReasonsService
{
    Task<List<PickReasonEntity>> GetAll();
    Task<List<PickReasonEntity>> GetByStockId(int stockPickId);
    Task<PickReasonEntity> Add(PickReasonDto pickReasonDto);
    Task<PickReasonEntity> Update(PickReasonDto pickReasonDto);
    Task<PickReasonEntity> Delete(int id);
}