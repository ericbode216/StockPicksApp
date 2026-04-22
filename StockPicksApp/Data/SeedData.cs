using Microsoft.EntityFrameworkCore;

public class SeedData
{
    public static void Seed(ModelBuilder builder)
    {
        builder
            .Entity<StockPickEntity>()
            .HasData(
                new List<StockPickEntity>
                {
                    new StockPickEntity
                    {
                        Id = 1,
                        StockTicker = "MSFT",
                        StockBuyDate = new DateTime(2025, 6,6),
                        StockBuyPrice = 400.03M,
                        IndexTicker = "VOO",
                        IndexBuyPrice = 1000.05M,
                        StockCurrentPrice = 410.10M,
                        IndexCurrentPrice = 1005.06M,
                    },
                    new StockPickEntity
                    {
                        Id = 2,
                        StockTicker = "AAPL",
                        StockBuyDate = new DateTime(2025, 6,6),
                        StockBuyPrice = 250.03M,
                        IndexTicker = "VOO",
                        IndexBuyPrice = 1000.05M,
                        StockCurrentPrice = 251.10M,
                        IndexCurrentPrice = 1005.06M,
                    },
                    new StockPickEntity
                    {
                        Id = 3,
                        StockTicker = "CPNG",
                        StockBuyDate = new DateTime(2025, 6,6),
                        StockBuyPrice = 19,
                        IndexTicker = "VOO",
                        IndexBuyPrice = 1000.05M,
                        StockCurrentPrice = 18.5M,
                        IndexCurrentPrice = 1005.06M,
                    },
                }
            );
        builder
            .Entity<PickReasonEntity>()
            .HasData(
                new List<PickReasonEntity>
                {
                    new PickReasonEntity
                    {
                        Id = 1,
                        StockId = 1,
                        Reason = "Microsoft is tied well into enterprises",
                    },
                    new PickReasonEntity
                    {
                        Id = 2,
                        StockId = 3,
                        Reason = "Coupang is the amazon of South Korea",
                    }
                }
            );
    }
}
