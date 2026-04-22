export type StockPick = {
    id:number;
    stockTicker:string;
    stockBuyDate:string;
    stockBuyPrice:number;
    indexTicker:string;
    indexBuyPrice:number;
    stockCurrentDate:string;
    stockCurrentPrice:number;
    indexCurrentPrice: number;
    stockTotalPercentGain: number | null;
    indexTotalPercentGain: number | null;
    stockAnnualPercentGain: number | null;
    indexAnnualPercentGain: number| null;
}