public class StockPickEntity
{
    public int Id { get; set; }
    public string StockTicker { get; set; }

    public DateTime StockBuyDate { get; set; }

    public decimal? StockBuyPrice { get; set; }

    public string IndexTicker { get; set;}

    public decimal? IndexBuyPrice { get; set; }

    public DateTime StockCurrentDate { get; set; }

    public decimal? StockCurrentPrice { get; set; }

    public decimal? IndexCurrentPrice { get; set; }

    public decimal? StockTotalPercentGain { get; set;}

    public decimal? IndexTotalPercentGain { get; set;}

    public decimal? StockAnnualPercentGain { get; set;}

    public decimal? IndexAnnualPercentGain { get; set;}

}
