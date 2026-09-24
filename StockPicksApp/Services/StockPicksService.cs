
public class StockPicksService : IStockPicksService
{
    private readonly IStockPicksRepository repository;
    private readonly IMarketDataService marketDataService;

    public StockPicksService(IStockPicksRepository repository, IMarketDataService marketDataService)
    {
        this.repository = repository;
        this.marketDataService = marketDataService;
    }

    public Task<List<StockPickEntity>> GetAll()
    {
        return repository.GetAll();
    }

     public Task<StockPickEntity> GetById(int id)
    {
        return repository.GetById(id);
    }

    public async Task<StockPickEntity> Add(StockPickAddDto stockPickDto)
    {
        var entity = new StockPickEntity();
        entity.StockTicker = stockPickDto.StockTicker;
        entity.StockBuyDate = DateTime.Parse(stockPickDto.StockBuyDate);

        ////Call 3rd Party API
        entity.StockBuyPrice = await marketDataService.GetHistoricalPrice(
            entity.StockTicker, 
            entity.StockBuyDate)
        ;

        if (entity.StockBuyPrice == -1)
        {
            throw new ArgumentException(
                $"Error finding historical price information for this stock ticker and date"
            );
        }
        
        entity.IndexTicker = stockPickDto.IndexTicker;
        entity.IndexBuyPrice = await marketDataService.GetHistoricalPrice(
            entity.IndexTicker,
            entity.StockBuyDate
        );
        if(entity.IndexBuyPrice == -1)
        {
            throw new ArgumentException(
                $"Error finding historical price information for this index ticker and date"
            );
        }

        ////Call 3rd Party API
        entity.StockCurrentPrice = await marketDataService.GetLatestPrice(entity.StockTicker);
        entity.IndexCurrentPrice = await marketDataService.GetLatestPrice(entity.IndexTicker);

        entity.StockCurrentDate = DateTime.Now;
        DoPercentCalculations(entity);
        return await repository.Add(entity);

    }

    public async Task<StockPickEntity> Update(StockPickUpdateDto stockPickDto)
    {
        var foundStockPick = await repository.GetById(stockPickDto.Id);
        if (foundStockPick == null)
        {
            throw new ArgumentException($"Error updating stockpick with id: {stockPickDto.Id}");
        }
        foundStockPick.StockTicker = stockPickDto.StockTicker;
        foundStockPick.StockBuyDate = DateTime.Parse(stockPickDto.StockBuyDate);
        foundStockPick.IndexTicker = stockPickDto.IndexTicker;

        //Call 3rd Party API
        foundStockPick.StockBuyPrice = await marketDataService.GetHistoricalPrice(
            foundStockPick.StockTicker,
            foundStockPick.StockBuyDate
        );

        foundStockPick.IndexBuyPrice = await marketDataService.GetHistoricalPrice(
            foundStockPick.IndexTicker,
            foundStockPick.StockBuyDate
        );

        //Call 3rd Party API
        foundStockPick.StockCurrentPrice = await marketDataService.GetLatestPrice(foundStockPick.StockTicker);
        foundStockPick.IndexCurrentPrice = await marketDataService.GetLatestPrice(foundStockPick.IndexTicker);
        foundStockPick.StockCurrentDate = DateTime.Now;
        DoPercentCalculations(foundStockPick);
        return await repository.Update(foundStockPick);



    }
    public async Task<StockPickEntity> Delete(int id)
    {
        Console.WriteLine("Inside service");
        var foundStockPick = await repository.GetById(id);
        if (foundStockPick == null)
        {
            throw new ArgumentException($"Error deleting stockpick with id: {id}");
        }
        Console.WriteLine("Inside service after if");
        return await repository.Delete(foundStockPick);

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